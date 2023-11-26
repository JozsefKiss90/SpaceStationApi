using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class EngineManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.ENGINE;

    public EngineManager(ILevelRepository levelRepository, int currentLevel) 
        : base(levelRepository, TYPE, currentLevel)
    {
    }

    public EngineManager(ILevelRepository levelRepository) 
        : this(levelRepository, 1)
    {
    }

    public int Speed => CurrentLevel.Effect;
}
