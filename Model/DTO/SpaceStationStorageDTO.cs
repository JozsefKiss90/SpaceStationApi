using System.Collections.Generic;
using System.Linq;
using SpaceShipAPI;

public record SpaceStationStorageDTO(Dictionary<ResourceType, int> Resources, int Level, int Capacity, int FreeSpace, bool FullyUpgraded)
{
  
}