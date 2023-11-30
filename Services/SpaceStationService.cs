using System.Security.Claims;
using DefaultNamespace;
using Microsoft.AspNetCore.Identity;
using SpaceshipAPI;
using SpaceShipAPI;
using SpaceShipAPI.Model.DTO;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Services;
using SpaceshipAPI.Spaceship.Model.Station;

public class SpaceStationService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ISpaceStationRepository _spaceStationRepository;
    private readonly ISpaceShipRepository _spaceShipRepository;
    private readonly ShipManagerFactory _shipManagerFactory;
    private readonly ILevelService _levelService;
    
    public SpaceStationService(
        UserManager<UserEntity> userManager,
        ISpaceStationRepository spaceStationRepository,
        ISpaceShipRepository spaceShipRepository,
        ShipManagerFactory shipManagerFactory,
        ILevelService levelService
        )
    {
        _userManager = userManager;
        _spaceStationRepository = spaceStationRepository;
        _spaceShipRepository = spaceShipRepository;
        _shipManagerFactory = shipManagerFactory;
        _levelService = levelService;
    }

    public async Task<SpaceStationDTO> GetBaseByIdAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetStationDTO();
    }
    

    public async Task<bool> AddResourcesAsync(long id, Dictionary<ResourceType, int> resources, ClaimsPrincipal user)
    { 
        var station = await GetStationByIdAndCheckAccessAsync(id, user);
        SpaceStationManager stationManager = new SpaceStationManager(station, _levelService);
        int value;
        foreach (var resource in resources.Keys)
        {
            stationManager.AddResource(resource, resources[resource]);
        }
        await _spaceStationRepository.UpdateAsync(station);
        return true;
    }

    public async Task<SpaceStationDataDTO> GetBaseDataForCurrentUserAsync(ClaimsPrincipal user)
    {
        var userEntity = await GetCurrentUser(user); 
        var station = await _spaceStationRepository.GetByUserAsync(userEntity);
        if (station == null)
        {
            throw new KeyNotFoundException("No station found for user");
        }
        return new SpaceStationDataDTO(station);
    }
    
    public async Task<long> AddShipAsync(long stationId, NewShipDTO newShipDTO, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        
        SpaceShip ship;
        if (newShipDTO.type == ShipType.MINER)
        {
            ship = MinerShipManager.CreateNewMinerShip(_levelService, newShipDTO.name, newShipDTO.color);
            stationManager.AddNewShip(ship, ShipType.MINER);
        }
        else if (newShipDTO.type == ShipType.SCOUT)
        {
            ship = ScoutShipManager.CreateNewScoutShip(_levelService, newShipDTO.name, newShipDTO.color);
            stationManager.AddNewShip(ship, ShipType.SCOUT);
        }
        else
        {
            throw new ArgumentException("Ship type not recognized");
        }
        
        await _spaceShipRepository.CreateAsync(ship);
        return ship.Id;
    }
    
    public async Task<Dictionary<ResourceType, int>> GetStorageUpgradeCostAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetStorageUpgradeCost();
    }

    
    public async Task<SpaceStationStorageDTO> GetStationStorageAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetStorageDTO();
    }

    public async Task<HangarDTO> GetStationHangarAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetHangarDTO();
    }
    
    public async Task<bool> UpgradeStorageAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        if (stationManager.UpgradeStorage())
        {
            await _spaceStationRepository.UpdateAsync(station);
            return true;
        }
        return false;
    }
    
    public async Task<Dictionary<ResourceType, int>> GetHangarUpgradeCostAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetHangarUpgradeCost();
    }
    
    public async Task<bool> UpgradeHangarAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        if (stationManager.UpgradeHangar())
        {
            await _spaceStationRepository.UpdateAsync(station);
            return true;
        }
        return false;
    }
    
    public async Task<bool> MoveResourceFromShipToStationAsync(long stationId, long shipId, Dictionary<ResourceType, int> resources, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var ship = await _spaceShipRepository.GetByIdAsync(shipId);
        if (ship == null || ship.SpaceStationId != station.Id)
        {
            throw new KeyNotFoundException("No such ship on this station");
        }

        var stationManager = new SpaceStationManager(station, _levelService);
        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(ship);
        if (stationManager.AddResourcesFromShip((MinerShipManager)spaceShipManager, resources))
        {
            await _spaceStationRepository.UpdateAsync(station);
            return true;
        }
        return false;
    }
    
    public async Task<Dictionary<ResourceType, int>> GetStoredResourcesAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await GetStationByIdAndCheckAccessAsync(stationId, user);
        var stationManager = new SpaceStationManager(station, _levelService);
        return stationManager.GetStoredResources();
    }

    private async Task<SpaceStation> GetStationByIdAndCheckAccessAsync(long stationId, ClaimsPrincipal user)
    {
        var station = await _spaceStationRepository.GetByIdAsync(stationId);
        if (station == null)
        {
            throw new KeyNotFoundException($"No station found with id {stationId}");
        }

        var currentUser = GetCurrentUser(user);
        var isAdmin = user.IsInRole("Admin"); 
        if (!isAdmin && currentUser.Id != int.Parse(station.User.Id))
        {
            throw new UnauthorizedAccessException("You don't have authority to access this station");
        }

        return station;
    }
    
    private string GetUserIdFromPrincipal(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    private async Task<UserEntity> GetCurrentUser(ClaimsPrincipal user)
    {
        var userId = GetUserIdFromPrincipal(user);
        return await _userManager.FindByIdAsync(userId);
    }

}
