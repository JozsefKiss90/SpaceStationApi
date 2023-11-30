using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using SpaceshipAPI;
using SpaceShipAPI.Model.DTO;
using SpaceshipAPI.Model.Ship;
using SpaceshipAPI.Spaceship.Model.Station;

public interface ISpaceStationRepository : IDisposable
{
    Task<IEnumerable<SpaceStationDTO>> GetAllAsync();
    Task<SpaceStation> GetByIdAsync(long id);
    Task<SpaceStation> GetByUserAsync(UserEntity user);
    Task CreateAsync(SpaceStation spaceStation);
    Task UpdateAsync(SpaceStation spaceStation);
    Task DeleteAsync(long id);
}
