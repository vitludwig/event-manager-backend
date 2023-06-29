using ElmahCore;
using ElmahCore.Mvc;
using EventApp.App.API.V1;
using EventApp.App.Dal;
using EventApp.App.Extensions;
using EventApp.App.Middleware;
using EventApp.App.Services.Event;
using EventApp.App.Services.Notification;
using EventApp.App.Services.Place;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace EventApp.App;

public class Startup
{
	private readonly Type[] _apiVersions = { typeof(V1) };
	private readonly string _corsPolicyName = "CorsPolicy";

	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddDbContext<EventAppContext>(opt =>
			// TODO: find out, why env variables are not loading
			opt.UseLazyLoadingProxies().UseNpgsql(Configuration.GetValue<string>("Pgsql"))
		// opt.UseLazyLoadingProxies().UseNpgsql("Host=localhost;Port=5432;Database=eventapp;Username=postgres;Password=postgres")
		);

		//Add main services.
		services.AddTransient<EventService>();
		services.AddTransient<PlaceService>();
		services.AddTransient<NotificationService>();
		services.AddHttpClient<PushServiceClient>();

		services.AddRouting(options => options.LowercaseUrls = true);

		services.AddCors(options =>
		{
			options.AddPolicy(name: _corsPolicyName, builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod();
			});
		});

		services.AddSignalR(options =>
		{
			options.EnableDetailedErrors = true;
		});

		/*var mvcBuilder = services.AddControllers();
		mvcBuilder.AddFluentValidation(fv =>
		{
			foreach (var apiVersion in _apiVersions)
			{
				fv.RegisterValidatorsFromAssemblyContaining(apiVersion);
			}

			fv.ImplicitlyValidateChildProperties = true;
			fv.DisableDataAnnotationsValidation = true;
		});
		mvcBuilder.AddJsonOptions(
			options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
			*/
		services.AddMapster();
		services.AddVersionServices(_apiVersions);

		services.AddElmah<XmlFileErrorLog>(options =>
		{
			//Log dir.
			var dir = Directory.GetCurrentDirectory();
			options.LogPath = Path.Combine(dir, "./Logs");

			//Restrict access. 
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
			{
				options.OnPermissionCheck = context =>
				{
					var elmahCookieName = Environment.GetEnvironmentVariable("ELMAH_COOKIE_NAME");
					var elmahCookiesPassword = Environment.GetEnvironmentVariable("ELMAH_COOKIE_PASSWORD");
					if (elmahCookieName == null || elmahCookiesPassword == null)
					{
						return false;
					}

					return context.Request.Cookies.TryGetValue(elmahCookieName, out var elmahPassword) && elmahPassword == elmahCookiesPassword;
				};
			}
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionsProvider)
	{
		//Fix datetime conversion refs: https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			// app.UseMultiVersionedSwagger(versionsProvider);
		}

		var useMigrations = Environment.GetEnvironmentVariable("USE_MIGRATIONS_IN_DEV");
		if (env.IsProduction() || useMigrations == "true")
		{
			app.UseMigrationsAutoStart();
		}

		//Setup X-Forwarded for proxy.
		var useXForwarded = Environment.GetEnvironmentVariable("USE_PROXY_X_FORWARDED");
		if (useXForwarded == "true")
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor |
								   ForwardedHeaders.XForwardedProto
			});
		}

		app.UseHttpsRedirection();
		app.UseRouting();

		//Allow cross-origins.
		app.UseCors(_corsPolicyName);
		app.UseElmah(); //Add elmah middleware

		app.UseMiddleware<RuntimeExceptionMiddleware>();

		app.UseEndpoints(endpoints =>
		{
			// endpoints.MapHealthChecksAsJson();
			var controllerBuilder = endpoints.MapControllers();

			// Disable authentication in dev mode.
			if (env.IsDevelopment() && Environment.GetEnvironmentVariable("DISABLE_AUTHENTICATION") == "true")
			{
				controllerBuilder.AllowAnonymous();
			}

			endpoints.MapV1Hubs();
		});
	}
}
