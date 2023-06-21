using System;

namespace EventApp.App.API.V1.DTO
{
	public class NotificationDto
	{
		public ENotificationType Type { get; set; }

		public ENotificationTarget Target { get; set; }

		public string? EventId { get; set; } = null;

		public string ChangedProperties { get; set; }

		public class Create : NotificationDto
		{

		}

		public class Read : NotificationDto
		{
			public Guid Id { get; set; }
			public DateTime Created { get; set; } = DateTime.Now;
		}

		public class HubGet
		{
			public DateTime? FromTime { get; set; }
			public Guid[]? EventIds { get; set; }
			public ENotificationType? Type { get; set; }
		}
	}
}
