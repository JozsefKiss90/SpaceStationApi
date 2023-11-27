using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;

public interface ISpaceShipRepository : IDisposable
{
    Task<IEnumerable<SpaceShip>> GetAllAsync();
    Task<SpaceShip> GetByIdAsync(long id);
    Task<IEnumerable<SpaceShip>> GetByStationIdAsync(long stationId); 
    Task CreateAsync(SpaceShip spaceShip);
    Task UpdateAsync(SpaceShip spaceShip);
    Task DeleteAsync(long id);
}