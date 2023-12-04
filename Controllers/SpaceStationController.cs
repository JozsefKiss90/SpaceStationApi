using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Services;
using SpaceShipAPI.Services;
using SpaceshipAPI.Spaceship.Model.Station;

namespace SpaceShipAPI.Controllers
{
    [ApiController]
    [Route("api/v1/base")]
    [Authorize]
    public class SpaceStationController : ControllerBase
    {
        private readonly ISpaceStationService _stationService;

        public SpaceStationController(ISpaceStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet("{baseId}")]
        public async Task<IActionResult> GetBaseById(long baseId)
        {
            var spaceStation = await _stationService.GetBaseByIdAsync(baseId, User);
            return Ok(spaceStation);
        }

        [HttpGet]
        public async Task<IActionResult> GetBaseDataForCurrentUser()
        {
            var spaceStationData = await _stationService.GetBaseDataForCurrentUserAsync(User);
            return Ok(spaceStationData);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(String name)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var SpaceStation = await _stationService.CreateAsync(name, User);
            return Ok(SpaceStation);
        }

        [HttpPost("{baseId}/add/resources")]
        [Authorize(Roles = "Admin")] // Restricts access to Admin role
        public async Task<IActionResult> AddResources(long baseId, [FromBody] Dictionary<ResourceType, int> resources)
        {
            var result = await _stationService.AddResourcesAsync(baseId, resources, User);
            return Ok(result);
        }

        [HttpPatch("{baseId}/add/resource-from-ship")]
        public async Task<IActionResult> MoveResourceFromShipToStation(long baseId, [FromQuery(Name = "ship")] long shipId, [FromBody] Dictionary<ResourceType, int> resources)
        {
            var result = await _stationService.MoveResourceFromShipToStationAsync(baseId, shipId, resources, User);
            return Ok(result);
        }

        [HttpPost("{baseId}/add/ship")]
        public async Task<IActionResult> AddShip(long baseId, [FromBody] NewShipDTO newShipDTO)
        {
            var shipId = await _stationService.AddShipAsync(baseId, newShipDTO, User);
            return Ok(shipId);
        }

        [HttpGet("{baseId}/storage")]
        public async Task<IActionResult> GetStationStorage(long baseId)
        {
            var storageDTO = await _stationService.GetStationStorageAsync(baseId, User);
            return Ok(storageDTO);
        }

        [HttpGet("{baseId}/storage/resources")]
        public async Task<IActionResult> GetStoredResources(long baseId)
        {
            var resources = await _stationService.GetStoredResourcesAsync(baseId, User);
            return Ok(resources);
        }

        [HttpGet("{baseId}/storage/upgrade/cost")]
        public async Task<IActionResult> GetStorageUpgradeCost(long baseId)
        {
            var cost = await _stationService.GetStorageUpgradeCostAsync(baseId, User);
            return Ok(cost);
        }

        [HttpPost("{baseId}/storage/upgrade")]
        [ActionName("UpgradeStorage")]
        public async Task<IActionResult> UpgradeStorage(long baseId)
        {
            var result = await _stationService.UpgradeStorageAsync(baseId, User);
            return Ok(result);
        }

        [HttpGet("{baseId}/hangar")]
        public async Task<IActionResult> GetStationHangar(long baseId)
        {
            var hangarDTO = await _stationService.GetStationHangarAsync(baseId, User);
            return Ok(hangarDTO);
        }

        [HttpGet("{baseId}/hangar/upgrade")]
        public async Task<IActionResult> GetHangarUpgradeCost(long baseId)
        {
            var cost = await _stationService.GetHangarUpgradeCostAsync(baseId, User);
            return Ok(cost);
        }

        [HttpPost("{baseId}/hangar/upgrade")]
        public async Task<IActionResult> UpgradeHangar(long baseId)
        {
            var result = await _stationService.UpgradeHangarAsync(baseId, User);
            return Ok(result);
        }
    }
}
