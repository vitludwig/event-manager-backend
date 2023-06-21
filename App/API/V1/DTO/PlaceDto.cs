using EventApp.API.V1.DTO;
using System;

namespace EventApp.App.API.V1.DTO
{
	public class PlaceDto
	{
		public string Name { get; set; } = "";


		public class Create : EventDto
		{

		}

		public class Read : EventDto
		{
			public Guid Id { get; set; }
		}

		public class Edit : EventDto
		{

		}
	}
}
