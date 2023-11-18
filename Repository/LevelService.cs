using SpaceShipAPI.Model;
using SpaceShipAPI.DTO;
using SpaceShipAPI.Repository;

namespace SpaceShipAPI.Service
{
    public class LevelService
    {
        private readonly ILevelRepository levelRepository;

        public LevelService(ILevelRepository levelRepository)
        {
            this.levelRepository = levelRepository;
        }

        public List<UpgradeableType> GetLevelTypes()
        {
            return Enum.GetValues(typeof(UpgradeableType)).Cast<UpgradeableType>().ToList();
        }

        public Level GetLevelByTypeAndLevel(UpgradeableType type, int level)
        {
            return levelRepository.GetLevelByTypeAndLevel(type, level)
                   ?? throw new ArgumentException($"{type} has no level {level}.");
        }

        public List<LevelDTO> GetLevelsByType(UpgradeableType type)
        {
            return levelRepository.GetLevelsByType(type)
                   .Select(level => new LevelDTO(level))
                   .ToList();
        }

        public LevelDTO UpdateLevelById(long id, NewLevelDTO newLevelDTO)
        {
            var level = levelRepository.FindById(id)
                        ?? throw new Exception("Level not found");
            level.Effect = newLevelDTO.Effect;
            level.Costs = new HashSet<LevelCost>(
                newLevelDTO.Cost.Select(kv => new LevelCost 
                { 
                    Resource = kv.Key, 
                    Amount = kv.Value, 
                    LevelId = level.Id 
                }));

            levelRepository.Save(level);
            return new LevelDTO(level);
        }

        public LevelDTO AddNewLevel(NewLevelDTO newLevelDTO)
        {
            var prevMaxLevel = levelRepository.GetLevelByTypeAndMax(newLevelDTO.Type, true);

            var newMaxLevel = new Level
            {
                LevelValue = prevMaxLevel == null ? 1 : prevMaxLevel.LevelValue + 1,
                Type = newLevelDTO.Type,
                Effect = newLevelDTO.Effect,
                Costs = new HashSet<LevelCost>(
                    newLevelDTO.Cost.Select(kv => new LevelCost 
                    { 
                        Resource = kv.Key, 
                        Amount = kv.Value
                        // LevelId is not set here
                    })),
                Max = true
            };

            // Update previous max level if it exists
            if (prevMaxLevel != null)
            {
                prevMaxLevel.Max = false;
                levelRepository.Save(prevMaxLevel);
            }

            // Save newMaxLevel to the database
            levelRepository.Save(newMaxLevel);

            // Return the DTO, now newMaxLevel has an Id assigned by the database
            return new LevelDTO(newMaxLevel);
        }


        public bool DeleteLastLevelOfType(UpgradeableType type)
        {
            var maxLevel = levelRepository.GetLevelByTypeAndMax(type, true)
                          ?? throw new InvalidOperationException($"No max level has been set for {type} type");
            if (maxLevel.LevelValue == 1)
            {
                throw new ArgumentException("The first level can't be deleted.");
            }
            var newMaxLevel = levelRepository.GetLevelByTypeAndLevel(type, maxLevel.LevelValue - 1)
                              ?? throw new InvalidOperationException($"Level sequence is incorrect for {type} type");
            newMaxLevel.Max = true;
            levelRepository.Delete(maxLevel);
            levelRepository.Save(newMaxLevel);
            return true;
        }
    }
}
