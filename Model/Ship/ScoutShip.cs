using SpaceshipAPI.Model.Ship;

using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShipAPI.Model.Ship
{
    [Table("scoutship")]
    public class ScoutShip : SpaceShip
    {
        public int ScannerLevel { get; set; }
    }
}