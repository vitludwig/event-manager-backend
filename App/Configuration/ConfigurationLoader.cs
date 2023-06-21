using EventApp.App.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EventApp.App.Configuration;

public static class ConfigurationLoader
{
	public static IConfigurationRoot Configure()
	{
		//Load variables from .env
		{
			var root = Directory.GetCurrentDirectory();
			DotEnvLoader.Load(Path.Combine(root, ".env"));
			DotEnvLoader.Load(Path.Combine(root, ".env.dev"));
		}

		Environment.SetEnvironmentVariable("Pgsql",
			"Host=" + System.Environment.GetEnvironmentVariable("POSTGREE_HOST") + ";" +
			"Database=" + System.Environment.GetEnvironmentVariable("POSTGREE_DB") + ";" +
			"Username=" + System.Environment.GetEnvironmentVariable("POSTGREE_USER") + ";" +
			"Password=" + System.Environment.GetEnvironmentVariable("POSTGREE_PASSWORD")
		);


		return new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddEnvironmentVariables()
			.Build();
	}
}
