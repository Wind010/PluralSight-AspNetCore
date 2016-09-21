using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();

        Trip GetTripName(string tripName);

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();


    }
}