using EventApp.App.Dal;
using EventApp.App.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.App.Services.Event
{
	public class EventService : IEventService
	{
		private readonly EventAppContext context;

		public EventService(
			EventAppContext _appContext
		)
		{
			context = _appContext;
		}

		public async Task<List<Dal.Entities.Event>> GetAll()
		{
			var events = await context.Events.Include(e => e.Place).ToListAsync();
			return events;
		}

		public async Task<Dal.Entities.Event> Create(Dal.Entities.Event value)
		{
			await context.Events.AddAsync(value);
			await context.SaveChangesAsync();

			return value;
		}

		public async Task<bool> DeleteEvent(Dal.Entities.Event value)
		{
			try
			{
				context.Events.Remove(value);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<Dal.Entities.Event> EditEvent(Dal.Entities.Event value)
		{
			context.Events.Update(value);
			await context.SaveChangesAsync();
			return value;
		}

		public async Task<Dal.Entities.Event> FindEventById(Guid eventId)
		{
			var foundEvent = await context.Events.AsQueryable().Where(x => x.Id == eventId).FirstOrDefaultAsync();

			if (foundEvent == null)
			{
				throw new NotFoundException($"Event {eventId} not found.");
			}

			return foundEvent;
		}
	}
}
