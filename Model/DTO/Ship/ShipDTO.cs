using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;


namespace SpaceShipAPI.Model.DTO.Ship
{
    public class ShipDTO
    {
        public long Id { get; }
        public string Name { get; set; }

        public ShipColor Color { get;  set; }
        public ShipType Type { get;  set; }
       // public long MissionId { get;  set; }

        public ShipDTO(SpaceShip ship)
            : this(ship.Id, ship.Name, ship.Color, GetShipType(ship))
        {
        }

        public ShipDTO(long id, string name, ShipColor color, ShipType type)
        {
            Id = id;
            Name = name;
            Color = color;
            Type = type;
            
        }
        
        public ShipDTO()
        {
        }

        private static ShipType GetShipType(SpaceShip ship)
        {
            if (ship is MinerShip)
            {
                return ShipType.MINER;
            }
            else if (ship is ScoutShip)
            {
                return ShipType.SCOUT;
            }
            else
            {
                throw new InvalidOperationException("Unrecognized ship type");
            }
        }
        
        private static long GetCurrentMissionId(Mission mission)
        {
            return mission?.Id ?? 0L;
        }
    }
}