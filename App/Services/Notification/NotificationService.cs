using EventApp.App.Dal;
using EventApp.App.Model;
using EventApp.App.Services.Event;
using EventApp.App.Services.Place;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventApp.App.Services.Notification
{
	public class NotificationService : INotificationService
	{

		private readonly LiteDB.LiteDatabase _db;
		private readonly LiteDB.LiteCollection<PushSubscription> _collection;
		private readonly EventAppContext appContext;
		private readonly PushServiceClient _pushClient;
		private readonly IPlaceService _placeService;
		private readonly IEventService _eventService;

		public partial class EventChangedProperties
		{

			public Guid? placeId { get; set; }
			public string? name { get; set; }
			public string? description { get; set; }
			public string? start { get; set; }
			public string? end { get; set; }

		}

		public NotificationService(
			EventAppContext _appContext,
			PushServiceClient pushClient,
			PlaceService placeService,
			EventService eventService
		)
		{
			appContext = _appContext;
			_pushClient = pushClient;
			_placeService = placeService;
			_eventService = eventService;

			var connectionString = @$"Filename=PushSubscriptionsStore.db; Connection=Shared;";
			_db = new LiteDB.LiteDatabase(connectionString);
			_collection = (LiteDB.LiteCollection<PushSubscription>)_db.GetCollection<PushSubscription>("subscriptions");

			_pushClient.DefaultAuthentication = new VapidAuthentication(
				"BM8bnspodQNmqUo03YgrvzhPiRZP5paOop_NK_SiRJfG8GW9DUw-H-FtXVQYtmLAMiakkFhc4KCdT6ep7InBbu0",
				"oOxhtspU0M-4dvui_leF9Lh13o177XGqTl85wsCnr2U"
			);
		}

		public async Task<Dal.Entities.Notification> AddNotification(Dal.Entities.Notification value)
		{
			await appContext.Notifications.AddAsync(value);
			await appContext.SaveChangesAsync();
			SendNotifications(value);

			return value;
		}

		/**
		 * Return notification after fromDate
		 */
		public IQueryable<Dal.Entities.Notification> GetNotifications()
		{
			return appContext.Notifications.AsQueryable();
		}

		public void AddSubscription(PushSubscription value)
		{
			_collection.Insert(value);
		}

		public void DeleteSubscription(string endpoint)
		{
			//_collection.Delete(subscription => subscription.Endpoint == endpoint);
		}

		private async Task SendNotifications(Dal.Entities.Notification value)
		{
			var title = "";

			if (value.Type is ENotificationType.Update && value.EventId is not null)
			{
				var notificationEvent = await _eventService.FindEventById(value.EventId.Value);
				title = $"Změna {notificationEvent.Name}";

			}
			else
			{
				title = "Nová zpráva";
			}

			EventChangedProperties changedProperties = JsonSerializer.Deserialize<EventChangedProperties>(value.ChangedProperties);
			var text = "";

			if (changedProperties.name is not null)
			{
				text += $"Název: {changedProperties.name}\r\n";
			}


			if (changedProperties.description is not null)
			{
				text += $"Popis: {changedProperties.description}\r\n";
			}

			if (changedProperties.start is not null)
			{
				var start = DateTime.Parse(changedProperties.start).ToString("dd.MM HH:mm");
				text += $"Start: {start}\r\n";
			}

			if (changedProperties.end is not null)
			{
				var end = DateTime.Parse(changedProperties.end).ToString("dd.MM HH:mm");
				text += $"Konec: {end}\r\n";
			}
			var data = new Dictionary<string, object>();
			var onClick = new { Default = new { operation = "openWindow", url = "/notifications" } };
			data.Add("onActionClick", onClick);
			PushMessage notification = new AngularPushNotification
			{
				Title = title,
				Body = text,
				Icon = "assets/icons/icon-96x96.png",
				Vibrate = { 200, 100, 200 },
				Data = data,
			}.ToPushMessage();

			foreach (PushSubscription subscription in _collection.FindAll())
			{
				// fire-and-forget
				_pushClient.RequestPushMessageDeliveryAsync(subscription, notification);
			}
		}
	}
}
