using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using SpaceshipAPI;
using SpaceShipAPI.Model.DTO;
using SpaceshipAPI.Spaceship.Model.Station;

public interface ISpaceStationRepository : IDisposable
{
    Task<IEnumerable<SpaceStationDTO>> GetAllAsync();
    Task<SpaceStationDTO> GetByIdAsync(long id);
    Task<SpaceStationDataDTO> GetByUserAsync(UserEntity user); // Added method
    Task CreateAsync(SpaceStation spaceStation);
    Task UpdateAsync(SpaceStation spaceStation);
    Task DeleteAsync(long id);
}
