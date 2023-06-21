using EventApp.App.Services.Event;
using EventApp.App.Services.Place;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.API.V1.Hubs
{
	public class EventsHub : Hub
	{
		private readonly EventService _eventService;
		private readonly PlaceService _placeService;

		public EventsHub(
			EventService eventService,
			PlaceService placeService
		)
		{
			_eventService = eventService;
			_placeService = placeService;
		}

		public async Task<List<App.Dal.Entities.Event>> getEvents()
		{
			return await _eventService.GetAll();
			// await Clients.All.SendAsync("allEvents", events);
		}

		public async Task<List<App.Dal.Entities.Place>> getPlaces()
		{
			return await _placeService.GetAll();
		}

		/*public async void sendEvent(EventDto.Read value)
		{
			await Clients.All.SendAsync("newEvent", value);
		}*/
	}
}
