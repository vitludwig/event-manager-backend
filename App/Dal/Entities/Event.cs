using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventApp.App.Dal.Entities
{
	[Table("events")]
	public class Event
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("name")]
		[Required]
		public string Name { get; set; } = "";

		[Column("description")]
		[Required]
		public string Description { get; set; } = "";

		[Column("image")]
		public string? Image { get; set; }

		[Column("start")]
		[Required]
		public DateTime Start { get; set; }

		[Column("end")]
		[Required]
		public DateTime End { get; set; }

		[Column("placeId")]
		[Required]
		public Guid PlaceId { get; set; }

		public virtual Place? Place { get; set; }

		[Column("favorit")]
		[Required]
		public bool Favorite { get; set; }

		[Column("type")]
		[Required]
		public string Type { get; set; } = "";

	}
}
