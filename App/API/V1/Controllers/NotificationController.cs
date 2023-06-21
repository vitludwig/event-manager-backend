using EventApp.API.V1.Hubs;
using EventApp.App.API.V1.DTO;
using EventApp.App.Dal.Entities;
using EventApp.App.Exceptions;
using EventApp.App.Services.Notification;
using Lib.Net.Http.WebPush;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.App.API.V1.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService notificationService;
		private readonly IMapper mapper;
		private readonly IHubContext<NotificationsHub> notificationHubContext;

		public NotificationController(
			NotificationService _notificationService,
			IMapper _mapper,
			IHubContext<NotificationsHub> _notificationhubContext
		)
		{
			notificationService = _notificationService;
			mapper = _mapper;
			notificationHubContext = _notificationhubContext;
		}

		[HttpGet]
		public async Task<ActionResult<NotificationDto.Read>> Get()
		{
			try
			{
				var notifications = await notificationService.GetNotifications().ToListAsync();

				return Ok(mapper.Map<List<NotificationDto.Read>>(notifications));
			}
			catch (NotFoundException e)
			{
				return NotFound(e);
			}
		}

		[HttpPost]
		public async Task<ActionResult<NotificationDto.Read>> Post([FromBody] NotificationDto.Create value)
		{
			var newNotification = mapper.Map<NotificationDto.Read>(
				await notificationService.AddNotification(mapper.Map<Notification>(value))
			);

			return Ok(newNotification);
		}

		[HttpPost("subscription")]
		public void AddSubscription([FromBody] PushSubscription subscription)
		{
			notificationService.AddSubscription(subscription);
		}

		[HttpDelete("subscription/{endpoint}")]
		public void Delete(string endpoint)
		{
			notificationService.DeleteSubscription(endpoint);
		}
	}
}
