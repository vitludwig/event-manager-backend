using EventApp.API.V1.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace EventApp.App.Extensions;

public static class HubExtensions
{

	public static IEndpointRouteBuilder MapV1Hubs(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapHub<EventsHub>("/signalr/events");
		endpoints.MapHub<NotificationsHub>("/signalr/notifications");

		return endpoints;
	}

}
