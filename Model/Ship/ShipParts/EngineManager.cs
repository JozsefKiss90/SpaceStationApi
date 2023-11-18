using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class EngineManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.ENGINE;

    public EngineManager(LevelService levelService, int currentLevel) 
        : base(levelService, TYPE, currentLevel)
    {
    }

    public EngineManager(LevelService levelService) 
        : this(levelService, 1)
    {
    }

    public int Speed => CurrentLevel.Effect;
}
