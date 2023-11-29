using SpaceShipAPI.DTO;
using SpaceShipAPI.Model.Location;
using SpaceShipAPI.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SpaceShipAPI.Model.DTO;

namespace SpaceShipAPI.Services
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<LocationDTO>> GetAllLocationsForCurrentUserAsync(
            ClaimsPrincipal user, 
            List<ResourceType> resourceTypes, 
            int reserveGreaterThan)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var locations = await _locationRepository
                .FindAllByUserAndResourceTypeInAndResourceReserveGreaterThanAsync(
                    userId, 
                    resourceTypes, 
                    reserveGreaterThan);

            return locations.Select(location => new LocationDTO(location));
        }
    }
}