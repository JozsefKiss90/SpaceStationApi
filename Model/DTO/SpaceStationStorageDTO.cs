using System.Collections.Generic;
using System.Linq;
using SpaceShipAPI;
using SpaceshipAPI.Spaceship.Model.Station;

public record SpaceStationStorageDTO(Dictionary<ResourceType, int> Resources, int Level, int Capacity, int FreeSpace, bool FullyUpgraded);

public class SpaceStationStorageDTOFactory
{
    public static SpaceStationStorageDTO Create(StationStorageManager storageManager)
    {
        var resources = storageManager.GetStoredResources();
        var level = storageManager.GetCurrentLevel();
        var capacity = storageManager.GetCurrentCapacity();
        var freeSpace = storageManager.GetCurrentAvailableStorageSpace();
        var fullyUpgraded = storageManager.IsFullyUpgraded();

        return new SpaceStationStorageDTO(resources, level, capacity, freeSpace, fullyUpgraded);
    }
}
