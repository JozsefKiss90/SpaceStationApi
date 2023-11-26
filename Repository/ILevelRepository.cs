using SpaceShipAPI.DTO;

namespace SpaceShipAPI.Repository;

using SpaceShipAPI.Model;
using System;
using System.Collections.Generic;

public interface ILevelRepository
{
    List<UpgradeableType> GetLevelTypes();

    Level GetLevelByTypeAndLevel(UpgradeableType type, int level);
    Level GetLevelByTypeAndMax(UpgradeableType type, bool isMax);

    List<LevelDTO> GetLevelsByType(UpgradeableType type);

    LevelDTO UpdateLevelById(long id, NewLevelDTO newLevelDTO);

    LevelDTO AddNewLevel(NewLevelDTO newLevelDTO);

    bool DeleteLastLevelOfType(UpgradeableType type);
    
    Level FindById(long id);

    void Save(Level level);

    void Delete(Level level);
}

