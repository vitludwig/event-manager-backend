using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.App.Services.Event
{
	public interface IEventService
	{
		public Task<List<Dal.Entities.Event>> GetAll();

		public Task<Dal.Entities.Event> Create(Dal.Entities.Event value);

		public Task<Dal.Entities.Event> FindEventById(Guid eventId);

		public Task<bool> DeleteEvent(Dal.Entities.Event value);

		public Task<Dal.Entities.Event> EditEvent(Dal.Entities.Event value);
	}
}
