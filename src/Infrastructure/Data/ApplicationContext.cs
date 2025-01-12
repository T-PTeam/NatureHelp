using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationContext : DbContext
{
    public DbSet<Report> Reports { get; set; }
    public DbSet<Deficiency> Deficiencys { get; set; }
    public DbSet<Domain.Models.Nature.Location> NatLocations { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<Laboratory> Laboratories { get; set; }
    public DbSet<Domain.Models.Organization.Location> OrgLocations { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        :base(options) { }
}
