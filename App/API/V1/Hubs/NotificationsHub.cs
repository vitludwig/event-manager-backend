using EventApp.App.API.V1.DTO;
using EventApp.App.Services.Notification;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventApp.API.V1.Hubs
{
	public class NotificationsHub : Hub
	{
		private readonly NotificationService notificationService;

		public NotificationsHub(NotificationService _notificationService)
		{
			notificationService = _notificationService;
		}

		public List<App.Dal.Entities.Notification> getNotifications(NotificationDto.HubGet data)
		{
			//NESPUSTI SE TAHLE METODA PROTOZE FE NEPOSILA SPRAVNE PARAMETRY NEBO SE TADY SPATNE PARSUJI
			var notifications = notificationService.GetNotifications();

			if (data.FromTime is not null)
			{
				notifications = notifications.Where(n => DateTime.Compare(data.FromTime.Value, n.Created) < 0);
			}

			if (data.EventIds?.Length > 0)
			{
				notifications = notifications.Where(n => n.EventId.HasValue && data.EventIds.Contains(n.EventId.Value));
			}

			if (data.Type is not null)
			{
				notifications = notifications.Where(n => n.Type.Equals(data.Type.Value));
			}
			return notifications.ToList();
		}
	}
}
