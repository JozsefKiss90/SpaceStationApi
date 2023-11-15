using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefaultNamespace;

public class MiningMission : Mission
{
    [ForeignKey("LocationId")]
    public Location Location { get; set; }
}
