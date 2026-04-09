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

        builder.Entity<Achievement>()
            .HasIndex(a => a.Code)
            .IsUnique();

        builder.Entity<UserDailyVisit>()
            .HasIndex(v => new { v.UserId, v.VisitDate })
            .IsUnique();

        builder.Entity<DeficiencyConfirmation>()
            .HasIndex(c => new { c.UserId, c.DeficiencyId, c.DeficiencyType })
            .IsUnique();

        builder.Entity<UserAchievement>()
            .HasKey(ua => new { ua.UserId, ua.AchievementId });

        builder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserAchievement>()
            .HasOne(ua => ua.Achievement)
            .WithMany()
            .HasForeignKey(ua => ua.AchievementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AppDictEntry>(e =>
        {
            e.ToTable("dict");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.EntryKey).IsUnique();
            e.Property(x => x.EntryKey).HasColumnName("key").HasMaxLength(256);
            e.Property(x => x.ValueJson).HasColumnName("value");
        });

        var ach1 = new Guid("a0000001-0000-4000-8000-000000000001");
        var ach2 = new Guid("a0000001-0000-4000-8000-000000000002");
        var ach3 = new Guid("a0000001-0000-4000-8000-000000000003");
        var ach4 = new Guid("a0000001-0000-4000-8000-000000000004");
        var ach5 = new Guid("a0000001-0000-4000-8000-000000000005");
        var ach6 = new Guid("a0000001-0000-4000-8000-000000000006");
        var ach7 = new Guid("a0000001-0000-4000-8000-000000000007");
        builder.Entity<Achievement>().HasData(
            new Achievement
            {
                Id = ach1,
                Code = "first_report",
                Title = "First report",
                Description = "Create your first deficiency report",
                Icon = "eco",
                RuleType = AchievementRuleType.TotalReportsCount,
                TargetInt = 1,
                SortOrder = 1,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach2,
                Code = "water_5",
                Title = "Water guardian",
                Description = "Create 5 water deficiency reports",
                Icon = "water_drop",
                RuleType = AchievementRuleType.WaterReportsCount,
                TargetInt = 5,
                SortOrder = 2,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach3,
                Code = "soil_3",
                Title = "Soil scientist",
                Description = "Create 3 soil deficiency reports",
                Icon = "grass",
                RuleType = AchievementRuleType.SoilReportsCount,
                TargetInt = 3,
                SortOrder = 3,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach4,
                Code = "xp_500",
                Title = "Dedicated contributor",
                Description = "Reach 500 total XP",
                Icon = "stars",
                RuleType = AchievementRuleType.TotalXp,
                TargetInt = 500,
                SortOrder = 4,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach5,
                Code = "level_3",
                Title = "Rising star",
                Description = "Reach level 3",
                Icon = "trending_up",
                RuleType = AchievementRuleType.CurrentLevel,
                TargetInt = 3,
                SortOrder = 5,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach6,
                Code = "confirm_5_others",
                Title = "Community voice",
                Description = "Confirm 5 deficiencies created by others",
                Icon = "verified",
                RuleType = AchievementRuleType.ConfirmationsGivenCount,
                TargetInt = 5,
                SortOrder = 6,
                IsActive = true,
            },
            new Achievement
            {
                Id = ach7,
                Code = "five_confirmations_own",
                Title = "Trusted report",
                Description = "Have one of your reports confirmed by 5 people",
                Icon = "groups",
                RuleType = AchievementRuleType.DeficiencyGotFiveConfirmations,
                TargetInt = 1,
                SortOrder = 7,
                IsActive = true,
            });

        builder.Entity<Organization>().HasData(GenerateTestDataToDB.Organizations);
        builder.Entity<Laboratory>().HasData(GenerateTestDataToDB.Laboratories);
        builder.Entity<User>().HasData(GenerateTestDataToDB.Users);
        builder.Entity<Report>().HasData(GenerateTestDataToDB.Reports);
        builder.Entity<SoilDeficiency>().HasData(GenerateTestDataToDB.SoilDeficiencies);
        builder.Entity<WaterDeficiency>().HasData(GenerateTestDataToDB.WaterDeficiencies);
    }
}
