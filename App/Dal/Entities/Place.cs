using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventApp.App.Dal.Entities
{
	[Table("places")]
	public class Place
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("name")]
		[Required]
		public string Name { get; set; } = "";
	}
}
