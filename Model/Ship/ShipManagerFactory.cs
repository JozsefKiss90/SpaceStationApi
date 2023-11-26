using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Repository;

namespace SpaceShipAPI.Model.Ship;

using SpaceShipAPI.Service;

public class ShipManagerFactory
{
    private readonly ILevelRepository levelRepository;

    public ShipManagerFactory(ILevelRepository levelRepository)
    {
        this.levelRepository = levelRepository;
    }

    public SpaceShipManager GetSpaceShipManager(SpaceShip ship)
    {
        switch (ship)
        {
            case MinerShip minerShip:
                return new MinerShipManager(levelRepository, minerShip);

            case ScoutShip scoutShip:
                return new ScoutShipManager(levelRepository, scoutShip);

            default:
                throw new ArgumentException("This ship type is not supported.");
        }
    }
}
