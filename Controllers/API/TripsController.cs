using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace TheWorld.Controllers.API
{
    using Models;
    using ViewModels;

    [Route("api/trips")]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllTrips());
        }

        [HttpPost]
        public IActionResult Add([FromBody] TripViewModels trip)
        {
            if (ModelState.IsValid)
            {
                return Created($"api/trips/{trip.Name}", trip);
            }

            // Returning just the ModelState is helpful for debugging in internal services.
            return BadRequest(ModelState);
        }

    }
}
