using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShipAPI.Database;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Services;

namespace SpaceShipAPI.Controllers;

[Route("api/spaceships")]
[ApiController]
[Authorize]
public class SpaceShipController : ControllerBase
{
    private readonly ShipService _shipService;
    private readonly ILogger<SpaceShipController> _logger;
    private readonly AppDbContext _userContext;

    public SpaceShipController(ShipService shipService, AppDbContext userContext, ILogger<SpaceShipController> logger)
    {
        _shipService = shipService;
        _userContext = userContext;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var spaceShips = await _shipService.GetAllShips(User);
        return Ok(spaceShips);
    }

    
    [HttpGet]
    public async Task<IActionResult> GetShipsByStationId(long stationId)
    {
        var spaceShips = await _shipService.GetShipsByStationAsync(stationId, User);
        return Ok(spaceShips);
    }

    [HttpGet("{id}", Name = "GetShip")] 
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var spaceShip = await _shipService.GetShipDetailsByIdAsync(id);
        if (spaceShip == null)
        {
            return NotFound();
        }

        return Ok(spaceShip);
    }

    [HttpPost, Authorize(Roles = "User")]  
    public async Task<IActionResult> CreateAsync([FromBody] NewShipDTO newShipDTO)
    {
        if (newShipDTO == null)
        {
            return BadRequest(); 
        }
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Value != "TokenForTheApiWithAuth");
        var userId = userIdClaim?.Value;
        var userExists = await _userContext.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return BadRequest("Invalid User ID");
        }

        if (string.IsNullOrEmpty(userId))
        { 
            return Unauthorized("User is not authenticated.");
        }
        _logger.LogInformation($"User ID: {userId}");

        SpaceShip spaceShip = await _shipService.CreateShip(newShipDTO, User);
        return Ok(spaceShip);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShipColors()
    {
        ShipColor[] colors = _shipService.getColors();
        return Ok(colors);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] SpaceShip spaceShip)
    {
        if (spaceShip == null || id != spaceShip.Id)
        {
            return BadRequest();
        }

        var existingSpaceShip = await _shipService.GetByIdAsync(id);
        if (existingSpaceShip == null)
        {
            return NotFound();
        }

        await _shipService.UpdateAsync(spaceShip);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var existingSpaceShip = await _shipService.GetByIdAsync(id);
        if (existingSpaceShip == null)
        {
            return NotFound();
        }

        await _shipService.DeleteAsync(id);

        return NoContent();
    }
}
