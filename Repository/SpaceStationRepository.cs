using DefaultNamespace;
using Microsoft.EntityFrameworkCore; 
using SpaceshipAPI;
using SpaceShipAPI.Database;
using SpaceShipAPI.Model.DTO;
using SpaceshipAPI.Model.Ship;
using SpaceshipAPI.Spaceship.Model.Station;

public class SpaceStationRepository : ISpaceStationRepository
{
    private readonly AppDbContext _context;

    public SpaceStationRepository(AppDbContext context)
    {
        _context = context; 
    }

    public async Task<IEnumerable<SpaceStationDTO>> GetAllAsync()
    {
        var spaceStation = await _context.SpaceStation
            .Include(s => s.Hangar)
            .Include(s => s.StoredResources)
            .ToListAsync();

        return spaceStation.Select(s => new SpaceStationDTO(
            id: s.Id,
            name: s.Name,
            hangar: null,
            storage: null
        ));
    }
    
    public async Task<SpaceStation> GetByIdAsync(long id)
    {
        var spaceStation = await _context.SpaceStation
            .Include(s => s.Hangar)
            .Include(s => s.StoredResources)
            .FirstOrDefaultAsync(s => s.Id == id);

        return spaceStation;
    }

    public async Task<SpaceStation> GetByUserAsync(UserEntity user)
    {
        var spaceStation = await _context.SpaceStation
            .FirstOrDefaultAsync(s => s.User == user);

        if (spaceStation == null)
            return null;

        return spaceStation;
    }
    public async Task CreateAsync(SpaceStation spaceStation)
    {
        _context.SpaceStation.Add(spaceStation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SpaceStation spaceStation)
    {
        _context.Entry(spaceStation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id) 
    {
        var spaceStation = await _context.SpaceStation.FindAsync(id);
        if (spaceStation != null)
        {
            _context.SpaceStation.Remove(spaceStation);
            await _context.SaveChangesAsync();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
