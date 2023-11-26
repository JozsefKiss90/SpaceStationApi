using SpaceShipAPI;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceshipAPI.Spaceship.Model.Station;

using SpaceShipAPI.Model;
using System.Collections.Generic;

public class StationStorageManager : AbstractStorageManager
{
    private static readonly UpgradeableType Type = UpgradeableType.STATION_STORAGE;

    public StationStorageManager(ILevelRepository levelRepository, int currentLevel, Dictionary<ResourceType, int> storedResources) 
        : base(levelRepository, Type, currentLevel, storedResources)
    {
    }

    public StationStorageManager(ILevelRepository levelRepository) 
        : this(levelRepository, 1, new Dictionary<ResourceType, int>())
    {
    }
}
