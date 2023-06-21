using EventApp.API.V1.DTO;
using EventApp.API.V1.Hubs;
using EventApp.App.Dal.Entities;
using EventApp.App.Exceptions;
using EventApp.App.Services.Event;
using EventApp.App.Services.Notification;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.API.V1.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class EventsController : ControllerBase
	{
		private readonly IEventService eventService;
		private readonly IMapper mapper;
		private readonly IHubContext<EventsHub> context;

		public EventsController(
			EventService _eventService,
			NotificationService _notificationService,
			IMapper _mapper,
			IHubContext<EventsHub> _context
		)
		{
			eventService = _eventService;
			mapper = _mapper;
			context = _context;
		}

		[HttpGet]
		public async Task<List<EventDto.Read>> Get()
		{
			var events = await eventService.GetAll();
			var eventsDto = mapper.Map<List<EventDto.Read>>(events);

			return eventsDto;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<EventDto.Read>> Get(Guid id)
		{
			try
			{
				var foundEvent = await eventService.FindEventById(id);
				return Ok(foundEvent);
			}
			catch (NotFoundException e)
			{
				return NotFound(e);
			}
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<EventDto.Read>> Post([FromBody] EventDto.Create value)
		{
			var newEvent = mapper.Map<EventDto.Read>(await eventService.Create(mapper.Map<Event>(value)));

			// TODO: send only to relevant clients?
			// await context.Clients.All.SendAsync("newEvent", newEvent);

			return Ok(newEvent);
		}

		/// <summary>
		/// Delete event. HARD delete!
		/// </summary>
		/// <param name="eventId"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{eventId}")]
		public async Task<ActionResult> Delete(Guid eventId)
		{
			var foundEvent = await eventService.FindEventById(eventId);

			if (await eventService.DeleteEvent(foundEvent))
			{
				return Ok();
			}

			return BadRequest("Event cannot be deleted.");
		}

		/// <summary>
		/// Edit event and add notification
		/// </summary>
		/// <param name="eventId"></param>
		/// <param name="eventDto"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPut("{eventId}")]
		public async Task<ActionResult<EventDto.Read>> Edit(Guid eventId, [FromBody] EventDto.Edit eventDto)
		{
			try
			{
				var foundEvent = await eventService.FindEventById(eventId);
				/*var eventToCompare = mapper.Map<EventDto.Edit>(foundEvent);
				var changedProps = Utils.GetChangedProperties<EventDto.Read>(eventDto, eventToCompare);*/

				mapper.Map(eventDto, foundEvent);
				var editedEvent = await eventService.EditEvent(foundEvent);

				var readEvent = mapper.Map<EventDto.Read>(editedEvent);
				await context.Clients.All.SendAsync("updateEvent", readEvent);

				/*var newNotification = new Notification()
				{
					Id = new Guid(),
					Type = ENotificationType.Update,
					Target = eventDto.notificationTarget,
					Created = DateTime.Now,
					EventId = eventId,
					ChangedProperties = changedProps,
				};
				await notificationService.AddNotification(newNotification);*/

				return Ok(readEvent);
			}
			catch (NotFoundException e)
			{
				return NotFound("Event with id " + eventId + " not found");
			}
		}
	}
}
