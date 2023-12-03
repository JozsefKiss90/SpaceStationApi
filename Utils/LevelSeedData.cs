namespace SpaceShipAPI.Utils;

using Microsoft.EntityFrameworkCore;
using SpaceShipAPI.Model;

public static class LevelSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        foreach (UpgradeableType type in Enum.GetValues(typeof(UpgradeableType)))
        {
            for (int level = 1; level <= 5; level++)
            {
                modelBuilder.Entity<Level>().HasData(
                    new Level
                    {
                        Id = GetLevelId(type, level),
                        Type = type,
                        LevelValue = level,
                        Max = (level == 5) 
                    }
                );
            }
        }
    }

    private static int GetLevelId(UpgradeableType type, int level)
    {
        return (int)type * 10 + level;
    }
}
