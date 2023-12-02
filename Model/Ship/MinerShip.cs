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
        public ICollection<StoredResource> StoredResources { get; set; }    }
}