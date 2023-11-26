using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class ScannerManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.SCANNER;

    public ScannerManager(ILevelRepository levelRepository, int level) 
        : base(levelRepository, TYPE, level)
    {
    }

    public int Efficiency => CurrentLevel.Effect;
}
