using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefaultNamespace;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public DateTime EndTime { get; set; }

    public EventType EventType { get; set; }

    public string EventMessage { get; set; }
}