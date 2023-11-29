using Microsoft.AspNetCore.Identity;
using SpaceshipAPI;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceShipAPI.Model.Mission;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship.ShipParts;

namespace SpaceShipAPI.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ShipService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ISpaceShipRepository _spaceShipRepository;
    private readonly ShipManagerFactory _shipManagerFactory;
    private readonly MissionFactory _missionFactory;
    private readonly ILevelService _levelService;
    private readonly MissionRepository _missionRepository;

    public ShipService(
        UserManager<UserEntity> userManager,
        ISpaceShipRepository spaceShipRepository,
        ShipManagerFactory shipManagerFactory,
        MissionFactory missionFactory,
        ILevelService levelService,
        MissionRepository missionRepository
        )
    {
        _userManager = userManager;
        _spaceShipRepository = spaceShipRepository;
        _shipManagerFactory = shipManagerFactory;
        _missionFactory = missionFactory;
        _levelService = levelService;
        _missionRepository = missionRepository;
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


    private async Task UpdateMissionIfExists(SpaceShipManager spaceShipManager)
    {
        var currentMission = spaceShipManager.GetCurrentMission();
        if (currentMission != null)
        {
            var missionManager = _missionFactory.GetMissionManager(currentMission);
            missionManager.SetShipManager(spaceShipManager);
            if (missionManager.UpdateStatus())
            {
                await _missionRepository.UpdateAsync(currentMission);
            }
        }
    }
    
    private async Task<SpaceShip> GetShipByIdAndCheckAccess(long id, ClaimsPrincipal user)
    {
        var ship = await _spaceShipRepository.GetByIdAsync(id);
        if (ship == null)
        {
            throw new KeyNotFoundException($"No ship found with id {id}");
        }

        var currentUser = await GetCurrentUserAsync(user);
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("You don't have authority to access this ship");
        }

        var isAdmin = user.IsInRole("Admin"); 
        if (!isAdmin && currentUser.Id != ship.UserId)
        {
            throw new UnauthorizedAccessException("You don't have authority to access this ship");
        }

        return ship;
    }


    
    private async Task<UserEntity> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
    {
        return await _userManager.GetUserAsync(userPrincipal);
    }

}
