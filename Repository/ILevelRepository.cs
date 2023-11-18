
using SpaceShipAPI.Model;  
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShipAPI.Repository
{
    public interface ILevelRepository
    {
        Level GetLevelByTypeAndLevel(UpgradeableType type, int level);

        IEnumerable<Level> GetLevelsByType(UpgradeableType type);

        Level GetLevelByTypeAndMax(UpgradeableType type, bool isMax);

        Level FindById(long id);

        void Save(Level level);

        void Delete(Level level);
    }
}
