using System;
using System.Collections.Generic;
using SpaceShipAPI;
using SpaceShipAPI.Model.DTO;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Services;
using SpaceShipAPI.Utils;

namespace SpaceshipAPI.Spaceship.Model.Station
{
    public interface ISpaceStationManager
    {
        SpaceStation CreateNewSpaceStation(string name);
        bool RemoveResources(Dictionary<ResourceType, int> cost);
        bool AddNewShip(SpaceShip ship, ShipType shipType);
        Dictionary<ResourceType, int> GetStoredResources();
        Dictionary<ResourceType, int> GetStorageUpgradeCost();
        Dictionary<ResourceType, int> GetHangarUpgradeCost();
        bool RemoveShip(SpaceShip ship);
        HashSet<SpaceShip> GetAllShips();
        bool HasShipAvailable(SpaceShip ship);
        bool UpgradeHangar();
        bool AddResourcesFromShip(MinerShipManager shipManager, Dictionary<ResourceType, int> resources);
        bool UpgradeStorage();
        bool AddResource(ResourceType resourceType, int quantity);
        SpaceStationDTO GetStationDTO();
        SpaceStationStorageDTO GetStorageDTO();
        HangarDTO GetHangarDTO();
        void CreateStorageIfNotExists();

        void CreateHangarIfNotExists();
    }
}