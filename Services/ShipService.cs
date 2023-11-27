using SpaceShipAPI.Model.DTO.Ship;
using SpaceShipAPI.Model.Mission;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship.ShipParts;

namespace SpaceShipAPI.Services;

using SpaceShipAPI.DTO;
using SpaceShipAPI.Model;
using SpaceShipAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ShipService
{
    private readonly ISpaceShipRepository _spaceShipRepository;
    // Assuming repositories and factories for ship management and missions are available
    private readonly ShipManagerFactory _shipManagerFactory;
    private readonly MissionFactory _missionFactory;
    private readonly ILevelService _levelService;

    public ShipService(
        ISpaceShipRepository spaceShipRepository,
        ShipManagerFactory shipManagerFactory,
        MissionFactory missionFactory,
        ILevelService levelService)
    {
        _spaceShipRepository = spaceShipRepository;
        _shipManagerFactory = shipManagerFactory;
        _missionFactory = missionFactory;
        _levelService = levelService;
    }

    public async Task<IEnumerable<ShipDTO>> GetShipsByStationAsync(long stationId, ClaimsPrincipal user)
    {
        // Authorization checks go here

        var ships = await _spaceShipRepository.GetByStationIdAsync(stationId);
        return ships.Select(ship => new ShipDTO(ship));
    }

    public async Task<ShipDetailDTO> GetShipDetailsByIdAsync(long id)
    {
        var ship = await _spaceShipRepository.GetByIdAsync(id);
        if (ship == null)
        {
            throw new KeyNotFoundException($"No ship found with id {id}");
        }

        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(ship);
        UpdateMissionIfExists(spaceShipManager);
        return spaceShipManager.GetDetailedDTO();
    }

    public async Task<ShipDetailDTO> UpgradeShipPartAsync(long id, ShipPart part)
    {
        var ship = await _spaceShipRepository.GetByIdAsync(id);
        if (ship == null)
        {
            throw new KeyNotFoundException($"No ship found with id {id}");
        }

        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(ship);
        var cost = spaceShipManager.GetUpgradeCost(part);
        // Assume stationManager is available to handle resource management
        // stationManager.RemoveResources(cost);
        spaceShipManager.UpgradePart(part);
        await _spaceShipRepository.UpdateAsync(ship);

        return spaceShipManager.GetDetailedDTO();
    }

    public async Task<bool> DeleteShipByIdAsync(long id)
    {
        var ship = await _spaceShipRepository.GetByIdAsync(id);
        if (ship == null)
        {
            throw new KeyNotFoundException($"No ship found with id {id}");
        }

        if (ship.CurrentMission != null)
        {
            throw new InvalidOperationException("Ship can't be deleted while on mission");
        }

        await _spaceShipRepository.DeleteAsync(id);
        return true;
    }

    // Other service methods...

    private void UpdateMissionIfExists(SpaceShipManager spaceShipManager)
    {
        var currentMission = spaceShipManager.GetCurrentMission();
        if (currentMission != null)
        {
            var missionManager = _missionFactory.GetMissionManager(currentMission);
            missionManager.SetShipManager(spaceShipManager);
            if (missionManager.UpdateStatus())
            {
                // Save updated mission
                // _missionRepository.Save(currentMission);
            }
        }
    }

    private SpaceShip GetShipByIdAndCheckAccess(long id, ClaimsPrincipal user)
    {
        // Logic to check access rights...
        // Return ship if user has access
    }
}
