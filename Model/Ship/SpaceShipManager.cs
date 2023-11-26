using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship.ShipParts;
using SpaceShipAPI.Repository;
using MissionNS = SpaceShipAPI.Model.Mission;

namespace SpaceShipAPI.Model.Ship;


public abstract class SpaceShipManager
{
    protected readonly ILevelRepository levelRepository;
    protected readonly SpaceShip spaceShip;
    protected ShieldManager shield;
    protected EngineManager engine;

    protected SpaceShipManager(ILevelRepository levelRepository, SpaceShip spaceShip)
    {
        this.levelRepository = levelRepository;
        this.spaceShip = spaceShip;
    }

    public SpaceShip GetShip()
    {
        return spaceShip;
    }

    public bool IsAvailable()
    {
        return spaceShip.CurrentMission == null;
    }

    public MissionNS.Mission GetCurrentMission()
    {
        return spaceShip.CurrentMission;
    }

    public void SetCurrentMission( MissionNS.Mission mission)
    {
        spaceShip.CurrentMission = mission;
    }

    public void EndMission()
    {
        spaceShip.CurrentMission = null;
    }

    public int GetShieldEnergy()
    {
        return spaceShip.ShieldEnergy;
    }

    public int GetShieldMaxEnergy()
    {
        CreateShieldIfNotExists();
        return shield.GetMaxEnergy();
    }

    public void RepairShield(int amount)
    {
        CreateShieldIfNotExists();
        shield.Repair(amount);
    }

    public void DamageShield(int amount)
    {
        CreateShieldIfNotExists();
        shield.Damage(amount);
    }

    public int GetSpeed()
    {
        CreateEngineIfNotExists();
        return engine.Speed;
    }

    protected void CreateShieldIfNotExists()
    {
        if (shield == null)
        {
            shield = new ShieldManager(levelRepository, spaceShip.ShieldLevel, spaceShip.ShieldEnergy);
        }
    }

    protected void CreateEngineIfNotExists()
    {
        if (engine == null)
        {
            engine = new EngineManager(levelRepository, spaceShip.EngineLevel);
        }
    }

    public abstract ShipDetailDTO GetDetailedDTO();

    public abstract HashSet<ShipPart> GetPartTypes();

    public abstract bool UpgradePart(ShipPart part);

    public abstract Dictionary<ResourceType, int> GetCost();

    public abstract Dictionary<ResourceType, int> GetUpgradeCost(ShipPart part);
}
