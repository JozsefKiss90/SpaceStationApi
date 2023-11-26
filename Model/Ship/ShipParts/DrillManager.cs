using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class DrillManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.DRILL;

    public DrillManager(ILevelRepository levelRepository, int currentLevel) 
        : base(levelRepository, TYPE, currentLevel)
    {
    }

    public DrillManager(ILevelRepository levelRepository) 
        : this(levelRepository, 1)
    {
    }

    public int Efficiency => CurrentLevel.Effect;
}
