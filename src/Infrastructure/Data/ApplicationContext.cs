using Domain.Enums;
using Domain.Models;
using Domain.Models.Analitycs;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Domain.Models.Profile;
using Domain.Models.Organization;
using Infrastructure.Providers;
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
    public DbSet<UserXpLedger> UserXpLedgers { get; set; }
    public DbSet<UserDailyVisit> UserDailyVisits { get; set; }
    public DbSet<DeficiencyConfirmation> DeficiencyConfirmations { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<AppDictEntry> DictEntries { get; set; }

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

        builder.Entity<UserAchievement>(e =>
        {
            e.HasKey(ua => new { ua.UserId, ua.AchievementId });
            e.HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(ua => ua.Achievement)
                .WithMany()
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
