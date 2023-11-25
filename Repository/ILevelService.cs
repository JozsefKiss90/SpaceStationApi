using SpaceShipAPI.DTO;

namespace SpaceShipAPI.Repository;

using SpaceShipAPI.Model;
using System;
using System.Collections.Generic;

public interface ILevelService
{
    List<UpgradeableType> GetLevelTypes();

    Level GetLevelByTypeAndLevel(UpgradeableType type, int level);

    List<LevelDTO> GetLevelsByType(UpgradeableType type);

    LevelDTO UpdateLevelById(long id, NewLevelDTO newLevelDTO);

    LevelDTO AddNewLevel(NewLevelDTO newLevelDTO);

    bool DeleteLastLevelOfType(UpgradeableType type);
}
