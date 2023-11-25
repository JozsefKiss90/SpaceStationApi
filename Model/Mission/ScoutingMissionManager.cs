using SpaceShipAPI.Model.DTO.Mission;
using SpaceShipAPI.Model.Location;
using SpaceShipAPI.Model.Ship;
namespace SpaceShipAPI.Model.Mission;

public class ScoutingMissionManager : MissionManager
{
    private readonly LocationDataGenerator locationDataGenerator;

    public ScoutingMissionManager(Mission mission, Random random, SpaceShipManager spaceShipManager) 
        : base(mission, random, spaceShipManager)
    {
    }

    public override MissionDetailDTO GetDetailedDTO()
    {
        return new ScoutingMissionDTO((ScoutingMission)mission);
    }

    public override bool AbortMission()
    {
        switch (mission.CurrentStatus)
        {
            case MissionStatus.OVER:
            case MissionStatus.ARCHIVED:
                throw new InvalidOperationException("Mission is already over.");
            case MissionStatus.RETURNING:
                throw new InvalidOperationException("Mission is already returning.");
        }

        var now = DateTime.UtcNow; // Using UtcNow instead of LocalDateTime.now(clock) from Java
        var abortedEvent = PopLastEvent();
        var abortEvent = new Event
        {
            EventType = EventType.ABORT,
            EndTime = now,
            EventMessage = abortedEvent.EventType == EventType.ACTIVITY_COMPLETE 
                           ? "Mission aborted by Command. Shutting down scan. Returning to station."
                           : "Mission aborted by Command. Returning to station."
        };

        PushNewEvent(abortEvent);
        mission.ApproxEndTime = now.AddSeconds(mission.TravelDurationInSecs);
        StartReturnTravel();
        return true;
    }

    protected override void AddStartEvent()
    {
        var scoutingMission = (ScoutingMission)mission;
        var startEvent = new Event
        {
            EndTime = mission.StartTime,
            EventType = EventType.START,
            EventMessage = $"Left station to discover new planet with {scoutingMission.TargetResource}."
        };
        PushNewEvent(startEvent);
    }

    protected override void StartActivity()
    {
        mission.CurrentStatus = MissionStatus.IN_PROGRESS;

        var lastEventTime = PeekLastEvent().EndTime;
        mission.CurrentObjectiveTime = lastEventTime.AddSeconds(mission.ActivityDurationInSecs);
        PeekLastEvent().EventMessage = "Reached target distance. Starting scan.";

        var activityEvent = new Event
        {
            EventType = EventType.ACTIVITY_COMPLETE,
            EndTime = lastEventTime.AddSeconds(mission.ActivityDurationInSecs)
        };
        PushNewEvent(activityEvent);
    }

    protected override void FinishActivity()
    {
        PeekLastEvent().EventMessage = "Finished scanning. Returning to station.";
        StartReturnTravel();
    }

    protected override void EndMission()
    {
        var location = GenerateLocation();
        var finalMessage = location == null
            ? "Returned to station.\nNo new planet discovered."
            : $"Returned to station.\nScan data decoded, discovered new planet: {location.Name}";
        PeekLastEvent().EventMessage = finalMessage;

        if (location != null)
        {
            ((ScoutingMission)mission).DiscoveredLocation = location;
        }
        
        mission.CurrentStatus = MissionStatus.OVER;
        this.EndMission();
    }

    private Location.Location GenerateLocation()
    {
        var scoutShipManager = (ScoutShipManager)shipManager;
        var scannerEfficiency = scoutShipManager.GetScannerEfficiency();
        var scanningHours = mission.ActivityDurationInSecs / 3600.0; // Convert seconds to hours
        var scoutingMission = (ScoutingMission)mission;
        var distance = scoutingMission.Distance;

        if (mission.ActivityDurationInSecs == 0 || 
            !locationDataGenerator.DeterminePlanetFound(scannerEfficiency, scanningHours, distance))
        {
            return null;
        }

        var prioritizingDistance = scoutingMission.PrioritizingDistance;
        var distanceFromStation = locationDataGenerator.DetermineDistance(scannerEfficiency, distance, prioritizingDistance);
        var resourceReserves = locationDataGenerator.DetermineResourceReserves(scannerEfficiency, scanningHours, !prioritizingDistance);
        var lastEventTime = PeekLastEvent().EndTime;

        return new Location.Location
        {
            Name = locationDataGenerator.DetermineName(),
            DistanceFromStation = distanceFromStation,
            Discovered = lastEventTime, // Assuming Discovered is a DateTime
            ResourceType = scoutingMission.TargetResource,
            ResourceReserve = resourceReserves,
            User = mission.User // Assuming User is a property of the mission
        };
    }

}
