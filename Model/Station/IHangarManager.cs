﻿using SpaceshipAPI.Model.Ship;

namespace SpaceshipAPI.Spaceship.Model.Station;

public interface IHangarManager
{
    int GetCurrentCapacity();
    int GetCurrentAvailableDocks();
    bool RemoveShip(SpaceShip ship);
    bool AddShip(SpaceShip ship);
    HashSet<SpaceShip> GetAllShips();
    bool HasShipAvailable(SpaceShip ship);
}
