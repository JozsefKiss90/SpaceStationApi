using DefaultNamespace;
using SpaceShipAPI;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Services;
using SpaceShipAPI.Utils;

namespace SpaceshipAPI.Spaceship.Model.Station;

using SpaceShipAPI.Model.Ship;
using System;
using System.Collections.Generic;

public class SpaceStationManager
{
    private readonly SpaceStation station;
    private readonly ILevelService levelService;
    private HangarManager hangar;
    private StationStorageManager storage;

    public SpaceStationManager(
        SpaceStation station, 
        ILevelService levelService
        )
    {
        this.station = station;
        this.levelService = levelService;
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

    public Dictionary<ResourceType, int> GetStoredResources()
    {
        CreateStorageIfNotExists();
    
        IDictionary<ResourceType, int> storedResources = ResourceUtility.ConvertToDictionary(storage.GetStoredResources());
        
        Dictionary<ResourceType, int> convertedResources = storedResources.ToDictionary(entry => entry.Key, entry => entry.Value);

        return convertedResources;
    }


    public Dictionary<ResourceType, int> GetStorageUpgradeCost()
    {
        CreateStorageIfNotExists();
        return storage.GetUpgradeCost();
    }

    public  Dictionary<ResourceType, int> GetHangarUpgradeCost() {
        CreateHangarIfNotExists();
        return hangar.GetUpgradeCost();
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
    
    public bool UpgradeHangar(){
        CreateHangarIfNotExists();
        Dictionary<ResourceType, int> cost = hangar.GetUpgradeCost();
        RemoveResources(cost);
        hangar.Upgrade();
        station.HangarLevel = hangar.GetCurrentLevel();
        return true;
    }
    
    public bool AddResourcesFromShip(MinerShipManager shipManager, Dictionary<ResourceType, int> resources) {
        if (HasShipAvailable(shipManager.GetShip()) && shipManager.HasResourcesInStorage(resources))
        { 
            int sum = resources.Values.Sum();
            CreateStorageIfNotExists();
            if (sum <= storage.GetCurrentAvailableStorageSpace()) {
                foreach (var resource in resources)
                {
                    AddResource( resource.Key, resource.Value);
                }
                return true;
            }
        }
        return false;
    }
    
    public bool UpgradeStorage() {
        CreateStorageIfNotExists();
        Dictionary<ResourceType, int> cost = storage.GetUpgradeCost();
        RemoveResources(cost);
        storage.Upgrade();
        station.StorageLevel = storage.GetCurrentLevel();
        return true;
    }

    public bool AddResource(ResourceType resourceType, int quantity)
    {
        CreateStorageIfNotExists();
        return storage.AddResource(resourceType, quantity);
    }

    public SpaceStationDTO GetStationDTO() {
        return new SpaceStationDTO(station.Id, station.Name, GetHangarDTO(), GetStorageDTO());
    }
    
    public SpaceStationStorageDTO GetStorageDTO() {
        CreateStorageIfNotExists();
        return SpaceStationStorageDTOFactory.Create(storage);
    }

    public HangarDTO GetHangarDTO() { 
        CreateHangarIfNotExists();
        return HangarDTOFactory.Create(hangar);
    }

    private void CreateHangarIfNotExists()
    {
        if (hangar == null)
        {
            hangar = new HangarManager(levelService, station.HangarLevel, new HashSet<SpaceShip>(station.Hangar));
        }
    }

    private void CreateStorageIfNotExists()
    {
        if (storage == null)
        {
            var storedResourcesDictionary = station.StoredResources
                .ToDictionary(sr => sr.ResourceType, sr => sr.Amount);

            storage = new StationStorageManager(levelService, station.StorageLevel, storedResourcesDictionary);
        }
    }
}
