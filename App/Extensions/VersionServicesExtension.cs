using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EventApp.App.Extensions;

public static class VersionServicesExtension
{
	public static void AddVersionServices(this IServiceCollection services, Type[] versions)
	{
		services.AddApiVersioning(config =>
		{
			config.DefaultApiVersion = new ApiVersion(1, 0);
			config.AssumeDefaultVersionWhenUnspecified = true;
			config.ReportApiVersions = true;
		});

		services.AddVersionedApiExplorer(setup =>
		{
			setup.GroupNameFormat = "'v'VV";
			setup.SubstituteApiVersionInUrl = true;
		});

		services.AddFluentValidationRulesToSwagger();
		services.AddSwaggerGen(c =>
		{
			c.EnableAnnotations(true, true);
			c.CustomSchemaIds(type =>
			{
				var nameParts = new List<string>();

				if (type.IsGenericType)
				{
					nameParts.Add(type.Name.Split("`").First());
					nameParts.AddRange(type.GenericTypeArguments.Select(t => t.FullName).ToArray());
				}
				else
				{
					nameParts.Add(type.FullName);
				}

				return String.Join(".", nameParts.Select(name => name.Split(".").Last().Replace("+", ".")));
			});

			var projName = Assembly.GetExecutingAssembly().GetName().Name;
			var filePath = Path.Combine(AppContext.BaseDirectory, $"{projName}.xml");
			c.IncludeXmlComments(filePath);
		});
		//services.ConfigureOptions<ConfigureSwaggerOptions>();
	}
}