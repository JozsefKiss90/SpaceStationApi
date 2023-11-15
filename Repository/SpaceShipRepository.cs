﻿using Microsoft.EntityFrameworkCore;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;

public class SpaceShipRepository : ISpaceShipRepository
{
    private readonly DBContext _context;

    public SpaceShipRepository(DBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShipDTO>> GetAllAsync()
    {
        var spaceShips = await _context.Set<SpaceShip>().ToListAsync();
        return spaceShips.Select(s => new ShipDTO(s));
    }


    public async Task<ShipDTO> GetByIdAsync(long id)
    {
        var spaceShip = await _context.Set<SpaceShip>()
            //.Include(s => s.CurrentMission) // Include related data
            .FirstOrDefaultAsync(s => s.Id == id);

        return spaceShip != null ? new ShipDTO(spaceShip) : null;
    } 
    

    public async Task CreateAsync(SpaceShip spaceShip)
    {
        _context.Add(spaceShip);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateAsync(SpaceShip spaceShip)
    {
        _context.Entry(spaceShip).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var spaceShip = await _context.Set<SpaceShip>()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (spaceShip != null)
        {
            _context.Remove(spaceShip);
            await _context.SaveChangesAsync();
        }
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}