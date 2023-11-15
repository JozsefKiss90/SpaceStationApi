using SpaceshipAPI.Spaceship.Model.Station;

namespace SpaceshipAPI;

using Microsoft.AspNetCore.Identity; 
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserEntity : IdentityUser
{
   
    public SpaceStation SpaceStation { get; set; }
    
   // public Role Role { get; set; }

    // You can override any additional properties and methods specific to ASP.NET Identity as needed.
}
