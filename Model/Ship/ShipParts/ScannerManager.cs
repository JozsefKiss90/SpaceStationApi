﻿using SpaceShipAPI.Service;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public class ScannerManager : Upgradeable
{
    private static readonly UpgradeableType TYPE = UpgradeableType.SCANNER;

    public ScannerManager(LevelService levelService, int level) 
        : base(levelService, TYPE, level)
    {
    }

    public int Efficiency => CurrentLevel.Effect;
}
