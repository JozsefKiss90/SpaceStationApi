using System.Security.Claims;
using DefaultNamespace;
using Microsoft.AspNetCore.Identity;
using SpaceshipAPI;
using SpaceShipAPI.Model.DTO;
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

    public async Task<SpaceStationDataDTO> GetBaseDataForCurrentUserAsync(ClaimsPrincipal user)
    {
        var userEntity = await GetCurrentUser(user); // Await the result of GetCurrentUser
        var station = await _spaceStationRepository.GetByUserAsync(userEntity);
        if (station == null)
        {
            throw new KeyNotFoundException("No station found for user");
        }
        return new SpaceStationDataDTO(station);
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
