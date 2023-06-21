using Lib.Net.Http.WebPush;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.App.Services.Notification
{
	public interface INotificationService
	{
		public Task<Dal.Entities.Notification> AddNotification(Dal.Entities.Notification value);

		public IQueryable<Dal.Entities.Notification> GetNotifications();

		public void AddSubscription(PushSubscription value);

		public void DeleteSubscription(string endpoint);
	}
}
