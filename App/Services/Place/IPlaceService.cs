using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.App.Services.Place
{
	public interface IPlaceService
	{
		public Task<List<Dal.Entities.Place>> GetAll();

		public Task<Dal.Entities.Place> Create(Dal.Entities.Place value);

		public Task<Dal.Entities.Place> FindById(Guid placeId);

		public Task<bool> Delete(Dal.Entities.Place value);

		public Task<Dal.Entities.Place> Edit(Dal.Entities.Place value);
	}
}
