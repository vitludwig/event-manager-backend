using EventApp.App.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace EventApp.App;

public static class Program
{
	static int Main(string[] args)
	{
		var configuration = ConfigurationLoader.Configure();
		var webHost = CreateWebHostBuilder(args, configuration).Build();

		try
		{
			webHost.Run();
			return 0;
		}
		catch (Exception)
		{
			return 1;
		}
	}

	private static IHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(configure =>
			{
				configure.UseStartup<Startup>()
					.UseConfiguration(configuration);
			});
}