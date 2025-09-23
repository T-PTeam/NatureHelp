using Domain.Models;
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
    public DbSet<Laboratory> Laboratories { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Research> Researches { get; set; }
    public DbSet<ChangedModelLog> ChangedModelLogs { get; set; }
    public DbSet<DeficiencyAttachment> Attachments { get; set; }
    public DbSet<CommentMessage> Comments { get; set; }
    public DbSet<DeficiencyMonitoring> DeficiencyMonitoring { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Attachment>()
            .HasDiscriminator<string>("AttachmentType")
            .HasValue<DeficiencyAttachment>("DeficiencyAttachment");

        builder.Entity<User>().OwnsOne(u => u.DeficiencyMonitoringScheme);

        builder.Entity<User>()
            .HasOne(u => u.Organization)
            .WithMany()
            .HasForeignKey(u => u.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Test DATA
        //builder.Entity<Report>()
        //    .HasData(GenerateTestDataToDB.Reports);

        //builder.Entity<WaterDeficiency>()
        //    .HasData(GenerateTestDataToDB.WaterDeficiencies);

        //builder.Entity<SoilDeficiency>()
        //    .HasData(GenerateTestDataToDB.SoilDeficiencies);

        //builder.Entity<Laboratory>()
        //    .HasData(GenerateTestDataToDB.Laboratories);

        //builder.Entity<User>()
        //    .HasData(GenerateTestDataToDB.Users);

        //builder.Entity<Organization>()
        //    .HasData(GenerateTestDataToDB.Organizations);
    }
}
