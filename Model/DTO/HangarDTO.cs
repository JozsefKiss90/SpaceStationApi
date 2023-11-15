using System.Collections.Generic;
using System.Linq;
using SpaceShipAPI.Model.DTO.Ship;

public record HangarDTO(HashSet<ShipDTO> Ships, int Level, int Capacity, int FreeDocks, bool FullyUpgraded)
{
   
}