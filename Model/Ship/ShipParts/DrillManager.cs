using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class DrillManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.DRILL;

    public DrillManager(LevelService levelService, int currentLevel) 
        : base(levelService, TYPE, currentLevel)
    {
    }

    public DrillManager(LevelService levelService) 
        : this(levelService, 1)
    {
    }

    public int Efficiency => CurrentLevel.Effect;
}
