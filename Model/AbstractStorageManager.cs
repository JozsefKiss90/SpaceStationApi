﻿using SpaceShipAPI.Model.Exceptions;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using SpaceShipAPI.Model; // Assuming this namespace contains Upgradeable, ResourceType, LevelService

public abstract class AbstractStorageManager : Upgradeable
{
    protected Dictionary<ResourceType, int> StoredResources { get; }

    protected AbstractStorageManager(ILevelRepository levelRepository, UpgradeableType type, int level, Dictionary<ResourceType, int> storedResources)
        : base(levelRepository, type, level)
    {
        int totalResources = storedResources.Values.Sum();
        if (totalResources > CurrentLevel.Effect)
        {
            throw new ResourceCapacityExceededException($"Stored resources can't exceed {CurrentLevel.Effect} at this level");
        }
        StoredResources = storedResources;
    }

    public int GetCurrentCapacity()
    {
        return CurrentLevel.Effect;
    }

    public int GetCurrentAvailableStorageSpace()
    {
        return GetCurrentCapacity() - StoredResources.Values.Sum();
    }

    public Dictionary<ResourceType, int> GetStoredResources()
    {
        return StoredResources;
    }

    public bool AddResource(ResourceType resourceType, int quantity)
    {
        if (quantity < 0)
        {
            throw new NegativeResourceAdditionException();
        }
        if (quantity > GetCurrentAvailableStorageSpace())
        {
            throw new InsufficientStorageSpaceException();
        }
        StoredResources[resourceType] = StoredResources.GetValueOrDefault(resourceType) + quantity;
        return true;
    }

    public bool HasResource(ResourceType resourceType, int quantity)
    {
        return StoredResources.TryGetValue(resourceType, out int storedAmount) && storedAmount >= quantity;
    }

    public bool RemoveResource(ResourceType resourceType, int quantity)
    {
        if (quantity < 0)
        {
            throw new Exception("Can't remove negative resources.");
        }
        if (!HasResource(resourceType, quantity))
        {
            throw new Exception("Not enough resource.");
        }
        StoredResources[resourceType] -= quantity;
        return true;
    }
}
