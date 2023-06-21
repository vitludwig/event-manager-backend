using EventApp.App.Dal.Entities;
using System;

namespace EventApp.API.V1.DTO
{
	public class EventDto
	{
		public string Name { get; set; } = "";

		public string Description { get; set; } = "";

		public string Type { get; set; } = "";

		public DateTime Start { get; set; }

		public DateTime End { get; set; }

		public string Image { get; set; } = "";

		public bool Favorite { get; set; }

		public Guid PlaceId { get; set; }


		public class Create : EventDto
		{

		}

		public class Read : EventDto
		{
			public Guid Id { get; set; }
			public Place Place { get; set; }
		}

		public class Edit : EventDto
		{
			public ENotificationTarget notificationTarget { get; set; } = ENotificationTarget.Interested;
		}
	}
}
