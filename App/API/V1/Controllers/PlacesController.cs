using EventApp.API.V1.DTO;
using EventApp.App.API.V1.DTO;
using EventApp.App.Dal.Entities;
using EventApp.App.Exceptions;
using EventApp.App.Services.Place;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.App.API.V1.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class PlacesController : ControllerBase
	{
		private readonly IPlaceService placeService;
		private readonly IMapper mapper;

		public PlacesController(
			PlaceService _placeService,
			IMapper _mapper
		)
		{
			placeService = _placeService;
			mapper = _mapper;
		}

		// GET: api/<PlacesController>
		[HttpGet]
		public Task<List<Dal.Entities.Place>> Get()
		{
			return placeService.GetAll();
		}

		// GET api/<PlacesController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Dal.Entities.Place>> Get(Guid id)
		{
			try
			{
				var place = await placeService.FindById(id);
				return Ok(place);
			}
			catch (NotFoundException e)
			{
				return NotFound(e);
			}
		}

		// POST api/<PlacesController>
		[HttpPost]
		public async Task<ActionResult<EventDto.Read>> Post([FromBody] EventDto.Create value)
		{
			var newEvent = mapper.Map<EventDto.Read>(await placeService.Create(mapper.Map<Place>(value)));

			return Ok(newEvent);
		}

		/// <summary>
		/// Delete place. HARD delete!
		/// </summary>
		/// <param name="placeId"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{placeId}")]
		public async Task<ActionResult> Delete(Guid placeId)
		{
			var place = await placeService.FindById(placeId);

			if (await placeService.Delete(place))
			{
				return Ok();
			}

			return BadRequest("Event cannot be deleted.");
		}

		/// <summary>
		/// Edit event.
		/// </summary>
		/// <param name="placeId"></param>
		/// <param name="placeDto"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPut("{placeId}")]
		public async Task<ActionResult<EventDto.Read>> Edit(Guid placeId, [FromBody] PlaceDto.Edit placeDto)
		{
			try
			{
				var place = await placeService.FindById(placeId);
				mapper.Map(placeDto, place);

				place = await placeService.Edit(place);

				return Ok(mapper.Map<EventDto.Read>(place));
			}
			catch (NotFoundException e)
			{
				return NotFound("Place with id " + placeId + " not found");
			}
		}
	}
}
