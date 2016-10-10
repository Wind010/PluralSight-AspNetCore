
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TheWorld.Controllers.API
{
    using AutoMapper;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels;

    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s=> s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }


        [HttpPost]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel stop)
        {
            if (ModelState.IsValid)
            {
                var newStop = Mapper.Map<Stop>(stop);

                // Lookup the Geocodes
                var result = await _coordsService.GetCoordsAsync(newStop.Name);
                if (! result.Success)
                {
                    //_logger.LogError(result.Message);
                }
                else
                {
                    newStop.Latitude = result.Latitude;
                    newStop.Longitude = result.Longitude;
                }

                // Save to database
                _repository.AddStop(tripName, newStop);
                

                if (await _repository.SaveChangesAsync())
                {
                    // Returning the TripViewModel and not the entity itself (Trip) for security/encapsulation.
                    return Created($"api/trips/{tripName}/stops/{newStop.Name}", 
                        Mapper.Map<StopViewModel>(newStop));
                }
            }

            // DEBUGGING: Determine why ModelState is invalid:
            //var errors = ModelState.Values.SelectMany(v => v.Errors);

            // Returning just the ModelState is helpful for debugging in internal services.
            return BadRequest("Failed to save the stop.");
        }


    }
}
