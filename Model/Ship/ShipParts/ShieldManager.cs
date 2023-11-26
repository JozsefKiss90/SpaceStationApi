using SpaceShipAPI.Repository;
using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

using SpaceShipAPI.Model;
using System;

public class ShieldManager : Upgradeable 
{
    private static readonly UpgradeableType TYPE = UpgradeableType.SHIELD;
    private int currentEnergy;

    public ShieldManager(ILevelRepository levelRepository, int currentLevel, int currentEnergy) 
        : base(levelRepository, TYPE, currentLevel)
    {
        if (currentEnergy < 0)
        {
            throw new ArgumentException("Shield energy can't be lower than 0");
        }
        if (currentEnergy > CurrentLevel.Effect) //line 21
        {
            throw new Exception($"Shield energy can't be higher than {CurrentLevel.Effect} at this level");
        }
        this.currentEnergy = currentEnergy;
    }

    public ShieldManager(ILevelRepository levelRepository) 
        : base(levelRepository, TYPE, 1)
    {
        currentEnergy = CurrentLevel.Effect;
    }

    public int GetMaxEnergy()
    {
        return CurrentLevel.Effect;
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public void SetEnergyToMax()
    {
        currentEnergy = GetMaxEnergy();
    }

    public void Repair(int amount)
    {
        currentEnergy = Math.Min(currentEnergy + amount, GetMaxEnergy());
    }

    public void Damage(int amount)
    {
        currentEnergy = Math.Max(currentEnergy - amount, 0);
    }
}
