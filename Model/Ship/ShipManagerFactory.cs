using SpaceshipAPI.Model.Ship;

namespace SpaceShipAPI.Model.Ship;

using SpaceShipAPI.Service;

public class ShipManagerFactory
{
    private readonly LevelService levelService;

    public ShipManagerFactory(LevelService levelService)
    {
        this.levelService = levelService;
    }

    public SpaceShipManager GetSpaceShipManager(SpaceShip ship)
    {
        switch (ship)
        {
            case MinerShip minerShip:
                return new MinerShipManager(levelService, minerShip);

            case ScoutShip scoutShip:
                return new ScoutShipManager(levelService, scoutShip);

            default:
                throw new ArgumentException("This ship type is not supported.");
        }
    }
}
