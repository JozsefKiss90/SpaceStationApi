using Microsoft.EntityFrameworkCore;
using SpaceShipAPI.Database;
using SpaceShipAPI.Model.Location;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceShipAPI;
using SpaceShipAPI.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Location>> FindAllByUserAndResourceTypeInAndResourceReserveGreaterThanAsync(
        string userId, 
        IEnumerable<ResourceType> resourceTypes, 
        int reserveGreaterThan)
    {
        return await _context.Locations
            .Where(loc => loc.User.Id == userId && 
                          resourceTypes.Contains(loc.ResourceType) &&
                          loc.ResourceReserve > reserveGreaterThan)
            .ToListAsync();
    }
}