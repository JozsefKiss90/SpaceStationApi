using SpaceShipAPI;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceshipAPI.Spaceship.Model.Station;

using SpaceShipAPI.Model.Ship;
using System;
using System.Collections.Generic;

public class SpaceStationManager
{
    private readonly SpaceStation station;
    private readonly ILevelRepository levelRepository;
    private HangarManager hangar;
    private StationStorageManager storage;

    public SpaceStationManager(SpaceStation station, ILevelRepository levelRepository)
    {
        this.station = station;
        this.levelRepository = levelRepository;
    }

    public static SpaceStation CreateNewSpaceStation(string name)
    {
        var station = new SpaceStation
        {
            Name = name,
            HangarLevel = 1,
            Hangar = new HashSet<SpaceShip>(),
            StorageLevel = 1,
            StoredResources = new List<StoredResource>()
            
        }; 
        foreach (var resourceType in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
        {
            station.StoredResources.Add(new StoredResource
            {
                ResourceType = resourceType,
                Amount = 0 
            });
        }

        return station;
    }

    private bool HasEnoughResource(Dictionary<ResourceType, int> cost)
    {
        CreateStorageIfNotExists();
        foreach (var entry in cost)
        {
            if (!storage.HasResource(entry.Key, entry.Value))
                return false;
        }
        return true;
    }

    public bool RemoveResources(Dictionary<ResourceType, int> cost)
    {
        CreateStorageIfNotExists();
        if (HasEnoughResource(cost))
        {
            foreach (var resource in cost.Keys)
            {
                storage.RemoveResource(resource, cost[resource]);
            }
            return true;
        }
        throw new Exception("Not enough resource");
    }

    public bool AddNewShip(SpaceShip ship, ShipType shipType)
    {
        var cost = shipType.GetCost();
        CreateHangarIfNotExists();
        if (!HasEnoughResource(cost))
        {
            throw new Exception("Not enough resource");
        }
        else if (hangar.GetCurrentAvailableDocks() == 0)
        {
            throw new Exception("No more docks available");
        }
        return hangar.AddShip(ship) && RemoveResources(cost);
    }

    public bool RemoveShip(SpaceShip ship)
    {
        CreateHangarIfNotExists();
        return hangar.RemoveShip(ship);
    }

    public HashSet<SpaceShip> GetAllShips()
    {
        return new HashSet<SpaceShip>(station.Hangar);
    }

    public bool HasShipAvailable(SpaceShip ship)
    {
        CreateHangarIfNotExists();
        return hangar.HasShipAvailable(ship);
    }

    public bool AddResource(ResourceType resourceType, int quantity)
    {
        CreateStorageIfNotExists();
        return storage.AddResource(resourceType, quantity);
    }

    // ... Implement other methods based on your application's need

    private void CreateHangarIfNotExists()
    {
        if (hangar == null)
        {
            hangar = new HangarManager(levelRepository, station.HangarLevel, new HashSet<SpaceShip>(station.Hangar));
        }
    }

    private void CreateStorageIfNotExists()
    {
        if (storage == null)
        {
            var storedResourcesDictionary = station.StoredResources
                .ToDictionary(sr => sr.ResourceType, sr => sr.Amount);

            storage = new StationStorageManager(levelRepository, station.StorageLevel, storedResourcesDictionary);
        }
    }
}
