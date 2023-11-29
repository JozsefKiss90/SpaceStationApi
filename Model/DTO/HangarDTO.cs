using System.Collections.Generic;
using System.Linq;
using SpaceShipAPI.Model.DTO.Ship;
using SpaceshipAPI.Spaceship.Model.Station;

public record HangarDTO(HashSet<ShipDTO> Ships, int Level, int Capacity, int FreeDocks, bool FullyUpgraded);

public class HangarDTOFactory
{
   public static HangarDTO Create(HangarManager hangarManager)
   {
      var ships = GetAllShipDTOs(hangarManager);
      int level = hangarManager.GetCurrentLevel();
      int capacity = hangarManager.GetCurrentCapacity();
      int freeDocks = hangarManager.GetCurrentAvailableDocks();
      bool fullyUpgraded = hangarManager.IsFullyUpgraded();

      return new HangarDTO(ships, level, capacity, freeDocks, fullyUpgraded);
   }

   private static HashSet<ShipDTO> GetAllShipDTOs(HangarManager hangar)
   {
      // Assuming ShipDTO has a constructor that takes the ship type from hangar.
      // Convert the collection of ships to HashSet<ShipDTO>.
      return hangar.GetAllShips().Select(ship => new ShipDTO(ship)).ToHashSet();
   }
}
