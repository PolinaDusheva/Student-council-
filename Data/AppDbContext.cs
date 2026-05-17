using Microsoft.EntityFrameworkCore;
using StudentCouncil.Models;

namespace StudentCouncil.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // PR отдел
    public DbSet<NewsPost> NewsPosts { get; set; }
    public DbSet<ScheduledPost> ScheduledPosts { get; set; }

    // Проекти и събития
    public DbSet<CouncilEvent> CouncilEvents { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    // Спорт
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TeamEntry> TeamEntries { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Equipment> Equipments { get; set; }

    // Членове на СС
    public DbSet<Member> Members { get; set; }

    // Социална дейност
    public DbSet<Initiative> Initiatives { get; set; }
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<MentorPerson> MentorPersons { get; set; }
}
