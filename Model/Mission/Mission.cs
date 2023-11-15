﻿using SpaceshipAPI;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpaceshipAPI.Model.Ship;

public abstract class Mission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime CurrentObjectiveTime { get; set; }
    public DateTime ApproxEndTime { get; set; }
    
    [Column(TypeName = "nvarchar(255)")]
    public MissionStatus CurrentStatus { get; set; }
    public long TravelDurationInSecs { get; set; }
    public long ActivityDurationInSecs { get; set; }

    [ForeignKey("ShipId")]
    public SpaceShip Ship { get; set; }
    
    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
    
    //public ICollection<Event> Events { get; set; }
}
