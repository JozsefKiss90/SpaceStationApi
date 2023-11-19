using SpaceShipAPI.Model.DTO.Ship;
using SpaceShipAPI.Model.DTO.Ship.Part;
using SpaceShipAPI.Model.Ship.ShipParts;

namespace SpaceShipAPI.Model.Ship;

using System;
using System.Collections.Generic;
using SpaceShipAPI.Model;
using SpaceShipAPI.Service;

public class MinerShipManager : SpaceShipManager
{
    private static readonly HashSet<ShipPart> PARTS = new HashSet<ShipPart> { ShipPart.ENGINE, ShipPart.SHIELD, ShipPart.DRILL, ShipPart.STORAGE };
    private readonly MinerShip minerShip;
    private ShipStorageManager storage;
    private DrillManager drill;

    public MinerShipManager(LevelService levelService, MinerShip minerShip) 
        : base(levelService, minerShip)
    {
        this.minerShip = minerShip;
    }

    public static MinerShip CreateNewMinerShip(LevelService levelService, string name, ShipColor color)
    {
        MinerShip ship = new MinerShip
        {
            Name = name,
            Color = color,
            EngineLevel = 1,
            ShieldLevel = 1,
            ShieldEnergy = new ShieldManager(levelService, 1, 0).GetMaxEnergy(),
            DrillLevel = 1,
            StorageLevel = 1,
            StoredResources = new Dictionary<ResourceType, int>()
        };
        return ship;
    }

    public override ShipDetailDTO GetDetailedDTO()
    {
        CreateEngineIfNotExists();
        CreateShieldIfNotExists();
        CreateDrillIfNotExists();
        CreateStorageIfNotExists();
        return new MinerShipDTO(
            minerShip.Id,
            minerShip.Name,
            minerShip.Color,
            ShipType.MINER,
            minerShip.CurrentMission,
            new EngineDTO(engine),
            new ShieldDTO(shield),
            new DrillDTO(drill),
            new ShipStorageDTO(storage));
    }

    // Other methods...

    public override HashSet<ShipPart> GetPartTypes()
    {
        return PARTS;
    }

    public override Dictionary<ResourceType, int> GetUpgradeCost(ShipPart part)
    {
        switch (part)
        {
            case ShipPart.ENGINE:
                CreateEngineIfNotExists();
                return engine.GetUpgradeCost();
            case ShipPart.SHIELD:
                CreateShieldIfNotExists();
                return shield.GetUpgradeCost();
            case ShipPart.DRILL:
                CreateDrillIfNotExists();
                return drill.GetUpgradeCost();
            case ShipPart.STORAGE:
                CreateStorageIfNotExists();
                return storage.GetUpgradeCost();
            default:
                throw new Exception("No such part on this ship");
        }
    }

    public override bool UpgradePart(ShipPart part)
    {
        switch (part)
        {
            case ShipPart.ENGINE:
                CreateEngineIfNotExists();
                engine.Upgrade();
                minerShip.EngineLevel = engine.GetCurrentLevel();
                break;
            case ShipPart.SHIELD:
                CreateShieldIfNotExists();
                shield.Upgrade();
                shield.SetEnergyToMax();
                minerShip.ShieldLevel = shield.GetCurrentLevel();
                minerShip.ShieldEnergy = shield.GetCurrentEnergy();
                break;
            case ShipPart.DRILL:
                CreateDrillIfNotExists();
                drill.Upgrade();
                minerShip.DrillLevel = drill.GetCurrentLevel();
                break;
            case ShipPart.STORAGE:
                CreateStorageIfNotExists();
                storage.Upgrade();
                minerShip.StorageLevel = storage.GetCurrentLevel();
                break;
            default:
                throw new Exception("No such part on this ship");
        }
        return true;
    }

    public override Dictionary<ResourceType, int> GetCost()
    {
        return new Dictionary<ResourceType, int>(ShipType.MINER.GetCost());
    }

    private void CreateStorageIfNotExists()
    {
        if (storage == null)
        {
            storage = new ShipStorageManager(levelService, minerShip.StorageLevel, minerShip.StoredResources); 
        }
    }

    private void CreateDrillIfNotExists()
    {
        if (drill == null)
        {
            drill = new DrillManager(levelService, minerShip.DrillLevel);
        }
    }
}
