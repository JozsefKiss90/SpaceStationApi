﻿using SpaceShipAPI;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Services;

namespace SpaceshipAPI.Spaceship.Model.Station;

using SpaceShipAPI.Model;
using System.Collections.Generic;

public class StationStorageManager : AbstractStorageManager
{
    private static readonly UpgradeableType Type = UpgradeableType.STATION_STORAGE;

    public StationStorageManager(ILevelService levelService, int currentLevel, Dictionary<ResourceType, int> storedResources) 
        : base(levelService, Type, currentLevel, storedResources)
    {
    }

    public StationStorageManager(ILevelService levelService) 
        : this(levelService, 1, new Dictionary<ResourceType, int>())
    {
    }
}
