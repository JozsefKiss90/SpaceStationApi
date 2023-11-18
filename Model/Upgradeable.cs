using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model;

public abstract class Upgradeable
{
    private readonly LevelService levelService;
    protected Level CurrentLevel;

    protected Upgradeable(LevelService levelService, UpgradeableType type, int level)
    {
        this.levelService = levelService;
        this.CurrentLevel = levelService.GetLevelByTypeAndLevel(type, level);
    }

    public bool IsFullyUpgraded()
    {
        return CurrentLevel.Max;
    }
    
    
    public Dictionary<ResourceType, int> GetUpgradeCost()
    {
        if (IsFullyUpgraded())
        {
            throw new Exception("Already at max level");
        }
        else
        {
            int level = CurrentLevel.LevelValue;
            UpgradeableType type = CurrentLevel.Type;
            Level nextLevel = levelService.GetLevelByTypeAndLevel(type, level + 1);

            // Convert ICollection<LevelCost> to Dictionary<ResourceType, int>
            return nextLevel.Costs.ToDictionary(cost => cost.Resource, cost => cost.Amount);
        }
    }

    public bool Upgrade()
    {
        if (IsFullyUpgraded())
        {
           // throw new UpgradeNotAvailableException("Already at max level");
           throw new Exception("Already at max level");
        }
        else
        {
            int level = CurrentLevel.LevelValue;
            UpgradeableType type = CurrentLevel.Type;
            CurrentLevel = levelService.GetLevelByTypeAndLevel(type, level + 1);
            Console.WriteLine(CurrentLevel.Effect);
            return true;
        }
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel.LevelValue;
    }
}
