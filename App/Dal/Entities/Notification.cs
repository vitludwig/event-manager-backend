using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventApp.App.Dal.Entities
{
	[Table("notifications")]
	public class Notification
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("type")]
		[Required]
		public ENotificationType Type { get; set; }

		[Column("target")]
		[Required]
		public ENotificationTarget Target { get; set; }

		[Column("created")]
		[Required]
		public DateTime Created { get; set; } = DateTime.Now;

		[Column("eventId")]
		public Guid? EventId { get; set; } = null;

		[Column("changedProperties")]
		public string ChangedProperties { get; set; }
	}
}
