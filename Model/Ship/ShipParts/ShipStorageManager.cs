using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class ShipStorageManager : AbstractStorageManager
{
    private static readonly UpgradeableType TYPE = UpgradeableType.SHIP_STORAGE; 

    public ShipStorageManager(ILevelRepository levelRepository, int currentLevel, Dictionary<ResourceType, int> storedResources) 
        : base(levelRepository, TYPE, currentLevel, storedResources)
    {
    }

    public ShipStorageManager(ILevelRepository levelRepository) 
        : this(levelRepository, 1, new Dictionary<ResourceType, int>())
    {
    }
}
