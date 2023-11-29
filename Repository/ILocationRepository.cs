using SpaceShipAPI.Model.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceShipAPI.Repository
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> FindAllByUserAndResourceTypeInAndResourceReserveGreaterThanAsync(
            string userId, 
            IEnumerable<ResourceType> resourceTypes, 
            int reserveGreaterThan);
    }
}