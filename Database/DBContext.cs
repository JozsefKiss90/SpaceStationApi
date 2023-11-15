using Microsoft.EntityFrameworkCore;
using SpaceshipAPI.Model.Ship;
using SpaceShipAPI.Model.Ship;
using SpaceshipAPI.Spaceship.Model.Station;

public class DBContext : DbContext
{
    public DbSet<MinerShip> MinerShips { get; set; }
    public DbSet<ScoutShip> ScoutShips { get; set; }

    public DbSet<SpaceStation> SpaceStation { get; set; }
    public DbSet<StoredResource> StoredResources { get; set; }


    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpaceStation>()
            .HasMany(s => s.StoredResources)
            .WithOne(sr => sr.SpaceStation)
            .HasForeignKey(sr => sr.SpaceStationId)
            .IsRequired(false); 

        modelBuilder
            .Entity<StoredResource>()
            .Property(e => e.ResourceType)
            .HasConversion<string>(); // If you want to store the enum as a string in the database

        modelBuilder.Entity<SpaceShip>()
            .Property(b => b.UserId)
            .IsRequired(false);
    }
}