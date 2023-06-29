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

		[Column("name_en")]
		[Required]
		public string Name_EN { get; set; } = "";

		[Column("description")]
		[Required]
		public string Description { get; set; } = "";

		[Column("description_en")]
		[Required]
		public string Description_EN { get; set; } = "";

		[Column("image")]
		public string? Image { get; set; }

		[Column("start")]
		[Required]
		public string Start { get; set; }

		[Column("end")]
		[Required]
		public string End { get; set; }

		[Column("placeId")]
		[Required]
		public Guid PlaceId { get; set; }

		public virtual Place? Place { get; set; }

		[Column("type")]
		[Required]
		public EEventType Type { get; set; }

	}
}
