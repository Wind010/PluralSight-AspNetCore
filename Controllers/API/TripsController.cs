﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace TheWorld.Controllers.API
{
    using AutoMapper;
    using Models;
    using ViewModels;

    [Route("api/trips")]
    [Authorize]
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

            // Returning just the ModelState is helpful for debugging in internal services.
            return BadRequest("Failed to save the trip.");
        }

    }
}
