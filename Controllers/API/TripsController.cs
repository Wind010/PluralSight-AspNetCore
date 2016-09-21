using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace TheWorld.Controllers.API
{
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Models;
    using ViewModels;

    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllTrips();

                return Ok(Mapper.Map<IEnumerable<TripViewModels>>(results));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");

                return BadRequest("Error Occured");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TripViewModels trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    // Returning the TripViewModel and not the entity itself (Trip) for security/encapsulation.
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModels>(newTrip));
                }
            }


            // DEBUGGING: Determine why ModelState is invalid:
            //var errors = ModelState.Values.SelectMany(v => v.Errors);


            // Returning just the ModelState is helpful for debugging in internal services.
            return BadRequest("Failed to save the trip.");
        }

    }
}
