using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from the database.");

            return _context.Trips.ToList();
        }

        public Trip GetTripName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => string.Equals(t.Name, tripName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
        
        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            _logger.LogInformation($"Getting trips from the database for {name}");

            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName == name).ToList();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
