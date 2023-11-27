using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Repository;
using SpaceShipAPI.Services;

namespace SpaceShipAPI.Model.Ship;

public class ShipManagerFactory
{
    private readonly ILevelService levelService;

    public ShipManagerFactory(ILevelService levelService)
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
