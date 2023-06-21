using EventApp.App.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventApp.App.Extensions;

public static class MigrationsAutoStart
{
	public static void UseMigrationsAutoStart(this IApplicationBuilder app)
	{
		using (var scope = app.ApplicationServices.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<EventAppContext>();
			context.Database.Migrate();
		}
	}
}
