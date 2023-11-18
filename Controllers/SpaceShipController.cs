using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShipAPI.Database;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;

namespace SpaceShipAPI.Controllers;

[Route("api/spaceships")]
[ApiController]
public class SpaceShipController : ControllerBase
{
    private readonly ISpaceShipRepository _spaceShipRepository;
    private readonly ILogger<SpaceShipController> _logger;
    private readonly AppDbContext _userContext;

    public SpaceShipController(ISpaceShipRepository spaceShipRepository, AppDbContext userContext, ILogger<SpaceShipController> logger)
    {
        _spaceShipRepository = spaceShipRepository;
        _userContext = userContext;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var spaceShips = await _spaceShipRepository.GetAllAsync();
        return Ok(spaceShips);
    }

    [HttpGet("{id}", Name = "GetShip")] 
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var spaceShip = await _spaceShipRepository.GetByIdAsync(id);
        if (spaceShip == null)
        {
            return NotFound();
        }

        return Ok(spaceShip);
    }

    [HttpPost, Authorize(Roles = "User")]  
    public async Task<IActionResult> CreateAsync([FromBody] ShipDTO shipDTO)
    {
        if (shipDTO == null)
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
     
        var minerShip = new MinerShip
        {
            Name = shipDTO.Name,
            Color = shipDTO.Color,
            EngineLevel = 0, 
            ShieldLevel = 0,
            ShieldEnergy = 0,
            DrillLevel = 0, 
            StorageLevel = 0,
            UserId = userId,
        };
        
        minerShip.SpaceStation = null;  
        //minerShip.User = null; 
        //minerShip.CurrentMission = null;

        await _spaceShipRepository.CreateAsync(minerShip);
        return CreatedAtRoute("GetShip", new { id = minerShip.Id }, minerShip);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] SpaceShip spaceShip)
    {
        if (spaceShip == null || id != spaceShip.Id)
        {
            return BadRequest();
        }

        var existingSpaceShip = await _spaceShipRepository.GetByIdAsync(id);
        if (existingSpaceShip == null)
        {
            return NotFound();
        }

        await _spaceShipRepository.UpdateAsync(spaceShip);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var existingSpaceShip = await _spaceShipRepository.GetByIdAsync(id);
        if (existingSpaceShip == null)
        {
            return NotFound();
        }

        await _spaceShipRepository.DeleteAsync(id);

        return NoContent();
    }
}
