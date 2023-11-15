using Microsoft.AspNetCore.Mvc;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;

namespace SpaceShipAPI.Controllers;

[Route("api/spaceships")]
[ApiController]
public class SpaceShipController : ControllerBase
{
    private readonly ISpaceShipRepository _spaceShipRepository;

    public SpaceShipController(ISpaceShipRepository spaceShipRepository)
    {
        _spaceShipRepository = spaceShipRepository;
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ShipDTO shipDTO)
    {
        if (shipDTO == null)
        {
            return BadRequest(); 
        }

        var minerShip = new MinerShip
        {
            Name = shipDTO.Name,
            Color = shipDTO.Color,
            EngineLevel = 0, 
            ShieldLevel = 0,
            ShieldEnergy = 0,
            DrillLevel = 0, 
            StorageLevel = 0,
      
        };
        
        minerShip.SpaceStation = null;  
        minerShip.User = null;
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
