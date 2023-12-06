using System.Security.Claims;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship.ShipParts;

namespace SpaceShipAPI.Services
{
    public interface IShipService
    {
        Task<SpaceShip> GetByIdAsync(long id);
        Task<IEnumerable<ShipDTO>> GetAllShips(ClaimsPrincipal user);
        Task<IEnumerable<ShipDTO>> GetShipsByStationAsync(long stationId, ClaimsPrincipal user);
        Task<ShipDetailDTO> GetShipDetailsByIdAsync(long id);
        Task<SpaceShip> CreateShip(NewShipDTO newShip, ClaimsPrincipal userPrincipal);
        Task<SpaceShip> UpdateAsync(SpaceShip ship);
        Task<ShipDetailDTO> UpgradeShipPartAsync(long id, ShipPart part);
        Task<bool> DeleteShipByIdAsync(long id);
        Task UpdateMissionIfExists(ISpaceShipManager spaceShipManager);
        ShipColor[] getColors();
        Task DeleteAsync(long id);
    }
}