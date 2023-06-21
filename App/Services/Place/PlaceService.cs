using EventApp.App.Dal;
using EventApp.App.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.App.Services.Place
{
	public class PlaceService : IPlaceService
	{
		private readonly EventAppContext context;

		public PlaceService(EventAppContext _appContext)
		{
			context = _appContext;
		}

		public async Task<Dal.Entities.Place> Create(Dal.Entities.Place value)
		{
			await context.Places.AddAsync(value);
			await context.SaveChangesAsync();

			return value;
		}

		public async Task<bool> Delete(Dal.Entities.Place value)
		{
			try
			{
				context.Places.Remove(value);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<Dal.Entities.Place> Edit(Dal.Entities.Place value)
		{
			context.Places.Update(value);
			await context.SaveChangesAsync();
			return value;
		}

		public async Task<Dal.Entities.Place> FindById(Guid placeId)
		{
			var foundEvent = await context.Places.AsQueryable().Where(x => x.Id == placeId).FirstOrDefaultAsync();

			if (foundEvent == null)
			{
				throw new NotFoundException($"Place {placeId} not found.");
			}

			return foundEvent;
		}

		public async Task<List<Dal.Entities.Place>> GetAll()
		{
			var places = await context.Places.ToListAsync();
			return places;
		}
	}
}
