using SpaceShipAPI.Model.Ship.ShipParts;

namespace SpaceShipAPI.Model.DTO.Ship.Part;

using System.Collections.Generic;

public record ShipStorageDTO(int Level, int MaxCapacity, Dictionary<ResourceType, int> Resources, bool FullyUpgraded)
{
    public ShipStorageDTO(ShipStorageManager shipStorageManager) 
        : this(shipStorageManager.GetCurrentLevel(), shipStorageManager.GetCurrentCapacity(), shipStorageManager.GetStoredResources(), shipStorageManager.IsFullyUpgraded())
    {
    }
}
