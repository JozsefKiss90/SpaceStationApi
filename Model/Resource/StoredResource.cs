
using SpaceShipAPI;
using SpaceshipAPI.Spaceship.Model.Station;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpaceShipAPI.Model.Ship;

public class StoredResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long SpaceStationId { get; set; }

    [ForeignKey("SpaceStationId")]
    public SpaceStation SpaceStation { get; set; }
    
    [ForeignKey("MinerShipId")]
    public MinerShip MinerShip { get; set; }
    public ResourceType ResourceType { get; set; }
    
    
    public int Amount { get; set; }
}