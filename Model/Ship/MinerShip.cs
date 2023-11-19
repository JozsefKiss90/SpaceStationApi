using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SpaceshipAPI.Model.Ship;

namespace SpaceShipAPI.Model.Ship
{
    [Table("minership")]
    public class MinerShip : SpaceShip
    {
        public int DrillLevel { get; set; }
        public int StorageLevel { get; set; }

        [InverseProperty("MinerShip")]
        public  Dictionary<ResourceType, int> StoredResources { get; set; }
    }
}