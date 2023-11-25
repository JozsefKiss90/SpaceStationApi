using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefaultNamespace;
using SpaceShipAPI.Model.Location;
using SpaceShipAPI.Model.Mission;

public class MiningMission : Mission
{
    [ForeignKey("LocationId")]
    public Location Location { get; set; }
}
