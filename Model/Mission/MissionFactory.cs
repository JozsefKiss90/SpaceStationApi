using SpaceShipAPI.Model.Location;
using SpaceShipAPI.Model.Ship;

namespace SpaceShipAPI.Model.Mission;

using SpaceShipAPI.Model;
using SpaceShipAPI.Model.Mission;
using SpaceShipAPI.Services;
using System;

public class MissionFactory
{
    private readonly ShipManagerFactory _shipManagerFactory;
    protected readonly Random random;
    
    public MissionFactory(ShipManagerFactory shipManagerFactory)
    {
        _shipManagerFactory = shipManagerFactory;
    }

    public MiningMission StartNewMiningMission(MinerShip spaceShip, Location.Location location, long activityDurationInSecs)
    {
        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(spaceShip);
        if (spaceShipManager is MinerShipManager minerShipManager)
        {
            return MiningMissionManager.StartMiningMission(minerShipManager, location, activityDurationInSecs);
        }
        else
        {
            throw new InvalidOperationException("Invalid ship manager.");
        }
    }

    public ScoutingMission StartNewScoutingMission(ScoutShip spaceShip, int distance, long activityDurationInSecs,
                                                   ResourceType targetResource, bool prioritizeDistance)
    {
        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(spaceShip);
        if (spaceShipManager is ScoutShipManager scoutShipManager)
        {
            return ScoutingMissionManager.StartScoutingMission(scoutShipManager, distance, activityDurationInSecs, targetResource, prioritizeDistance);
        }
        else
        {
            throw new InvalidOperationException("Invalid ship manager.");
        }
    }

    public MissionManager GetMissionManager(Mission mission)
    {
        var spaceShipManager = _shipManagerFactory.GetSpaceShipManager(mission.Ship);
        switch (mission)
        {
            case MiningMission miningMission:
                return new MiningMissionManager(miningMission, DateTime.Now,  (MinerShipManager)spaceShipManager);
            case ScoutingMission scoutingMission:
                return new ScoutingMissionManager(scoutingMission, random, (ScoutShipManager)spaceShipManager, new LocationDataGenerator(new Random()));
            default:
                throw new InvalidOperationException("Mission type is not recognized");
        }
    }
}
