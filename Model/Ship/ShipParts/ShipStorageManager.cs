using SpaceShipAPI.Services;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class ShipStorageManager : AbstractStorageManager
{
    private static readonly UpgradeableType TYPE = UpgradeableType.SHIP_STORAGE; 

    public ShipStorageManager(ILevelService levelService, int currentLevel, Dictionary<ResourceType, int> storedResources) 
        : base(levelService, TYPE, currentLevel, storedResources)
    {
    }

    public ShipStorageManager(ILevelService levelService) 
        : this(levelService, 1, new Dictionary<ResourceType, int>())
    {
    }
}
