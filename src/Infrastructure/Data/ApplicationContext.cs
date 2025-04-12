using Domain.Models.Analitycs;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationContext : DbContext
{
    public DbSet<Report> Reports { get; set; }
    public DbSet<WaterDeficiency> WaterDeficiencies { get; set; }
    public DbSet<SoilDeficiency> SoilDeficiencies { get; set; }
    public DbSet<Domain.Models.Nature.Location> NatLocations { get; set; }
    public DbSet<Laboratory> Laboratories { get; set; }
    public DbSet<Domain.Models.Organization.Location> OrgLocations { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Research> Researches { get; set; }
    public DbSet<ChangedModelLog> ChangedModelLogs { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Test DATA
        //builder.Entity<Report>()
        //    .HasData(GenerateTestDataToDB.Reports);

        //builder.Entity<WaterDeficiency>()
        //    .HasData(GenerateTestDataToDB.WaterDeficiencies);

        //builder.Entity<SoilDeficiency>()
        //    .HasData(GenerateTestDataToDB.SoilDeficiencies);

        //builder.Entity<Domain.Models.Organization.Location>()
        //    .HasData(GenerateTestDataToDB.Locations.Concat(GenerateTestDataToDB.PersonLocations));

        //builder.Entity<Laboratory>()
        //    .HasData(GenerateTestDataToDB.Laboratories);

        //builder.Entity<Domain.Models.Nature.Location>()
        //    .HasData(GenerateTestDataToDB.NatureLocations);

        //builder.Entity<User>()
        //    .HasData(GenerateTestDataToDB.Users);

        //builder.Entity<Organization>()
        //    .HasData(GenerateTestDataToDB.Organizations);
    }
}
