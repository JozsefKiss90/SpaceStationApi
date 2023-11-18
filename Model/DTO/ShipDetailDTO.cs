using SpaceShipAPI.Model.DTO.Ship;
using SpaceShipAPI.Model.DTO.Ship.Part;

namespace SpaceShipAPI.Model.Ship.ShipParts;

public abstract class ShipDetailDTO : ShipDTO
{
    public EngineDTO Engine { get; }
    public ShieldDTO Shield { get; }

    public ShipDetailDTO(long id, string name, ShipColor color, ShipType type, Mission mission, EngineDTO engine, ShieldDTO shield)
        : base(id, name, color, type, mission.Id)
    {
        Engine = engine;
        Shield = shield;
    }
}
