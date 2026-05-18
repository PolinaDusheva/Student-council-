// ============================================================
// ФАЙЛ: Data/AppDbContext.cs
// ЦЕЛЬ: Това е "мостът" между C# обектите и базата данни.
//       AppDbContext наследява DbContext от Entity Framework Core (EF Core).
//       Всяка публична DbSet<T> свойство представлява таблица в базата.
//
// КАК РАБОТИ EF CORE:
//   EF Core е ORM (Object-Relational Mapper) — позволява ни да работим
//   с базата данни чрез C# класове вместо директни SQL заявки.
//   DbContext пази връзката с базата, следи промените по обектите
//   (change tracking) и превежда LINQ заявки в SQL.
//
// ПРИМЕР на потока: db.Members.Where(m => m.Position == "Председател").ToList()
//   се превежда от EF Core до: SELECT * FROM Members WHERE Position = 'Председател'
// ============================================================

using Microsoft.EntityFrameworkCore;
using StudentCouncil.Models;

namespace StudentCouncil.Data;

// Наследяваме DbContext — основният клас на EF Core.
// Чрез него получаваме методите SaveChanges(), Database.EnsureCreated() и др.
public class AppDbContext : DbContext
{
    // Конструкторът приема DbContextOptions<AppDbContext> — обект с настройки:
    // кой provider да се ползва (SQLite, SQL Server...), connection string и т.н.
    // Подаваме настройките нагоре към базовия клас с : base(options).
    // ASP.NET Core DI контейнерът автоматично създава и подава тези настройки,
    // когато регистрираме услугата в Program.cs с AddDbContext<AppDbContext>().
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ── PR отдел ──────────────────────────────────────────────────────────
    // DbSet<T> представлява таблица в базата данни.
    // "NewsPosts" ще стане таблица с името "NewsPosts" в SQLite файла.
    // Чрез това свойство можем да правим: Db.NewsPosts.Add(...), .ToList(),
    // .Where(...), .Find(id) и т.н. — всичко се превежда до SQL.
    public DbSet<NewsPost> NewsPosts { get; set; }

    // ScheduledPost — планирани публикации за социалните мрежи.
    public DbSet<ScheduledPost> ScheduledPosts { get; set; }

    // ── Проекти и събития ─────────────────────────────────────────────────
    // CouncilEvent — събитие на студентския съвет (дата, място, участници).
    public DbSet<CouncilEvent> CouncilEvents { get; set; }

    // EventAttendee — записан участник за събитие (релация "един към много").
    //   Един CouncilEvent има много EventAttendee, свързани чрез EventId (FK).
    public DbSet<EventAttendee> EventAttendees { get; set; }

    // ProjectTask — задача, свързана с проект (отговорник, статус).
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    // ── Спорт ─────────────────────────────────────────────────────────────
    // Tournament — турнир с дата и вид спорт.
    // TeamEntry — отбор в даден турнир (свързан с Tournament чрез TournamentId).
    //   Това е релация "един към много" — един Tournament има много TeamEntry.
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TeamEntry> TeamEntries { get; set; }

    // Training — тренировъчно занятие (ден, час, зала, треньор).
    public DbSet<Training> Trainings { get; set; }

    // Equipment — спортен инвентар (може да е назаем или наличен).
    public DbSet<Equipment> Equipments { get; set; }

    // ── Членове на СС ─────────────────────────────────────────────────────
    // Member — член на студентския съвет (само: Id, Name, Position).
    // Тази таблица се използва в почти всеки модул за избор на хора.
    public DbSet<Member> Members { get; set; }

    // ── Социална дейност ──────────────────────────────────────────────────
    // Initiative — доброволческа инициатива.
    // Volunteer — записан доброволец към инициатива (релация "един към много").
    public DbSet<Initiative> Initiatives { get; set; }
    public DbSet<Volunteer> Volunteers { get; set; }

    // Poll — анкета с въпрос.
    // PollOption — вариант за отговор в анкета (релация "един към много").
    //   Един Poll има много PollOption, всеки с броячка за гласове.
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }

    // MentorPerson — участник в менторска програма.
    public DbSet<MentorPerson> MentorPersons { get; set; }
}
