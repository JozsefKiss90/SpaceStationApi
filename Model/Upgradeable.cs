using SpaceShipAPI.Model.Exceptions;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model;

public abstract class Upgradeable
{
    private readonly ILevelRepository levelRepository;
    protected Level CurrentLevel;

    protected Upgradeable(ILevelRepository levelRepository, UpgradeableType type, int level)
    {
        this.levelRepository = levelRepository;
        this.CurrentLevel = levelRepository.GetLevelByTypeAndLevel(type, level);
    }

    public bool IsFullyUpgraded()
    {
        return CurrentLevel.Max;
    }
    
    public Dictionary<ResourceType, int> GetUpgradeCost()
    {
        if (CurrentLevel == null)
        {
            throw new Exception("CurrentLevel is null");
        }

        if (IsFullyUpgraded())
        {
            throw new UpgradeNotAvailableException("Already at max level");        }

        int level = CurrentLevel.LevelValue;

        UpgradeableType type = CurrentLevel.Type;
        Level nextLevel = levelRepository.GetLevelByTypeAndLevel(type, level + 1);

        if (nextLevel == null)
        {
            throw new Exception("NextLevel is null");
        }

        return nextLevel.Costs.ToDictionary(cost => cost.Resource, cost => cost.Amount);
    }

    public bool Upgrade()
    {
        if (IsFullyUpgraded())
        { throw new Exception("Already at max level");
        }
        else
        {
            int level = CurrentLevel.LevelValue;
            UpgradeableType type = CurrentLevel.Type;
            CurrentLevel = levelRepository.GetLevelByTypeAndLevel(type, level + 1);
            Console.WriteLine(CurrentLevel.Effect);
            return true;
        }
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel.LevelValue;
    }
}
