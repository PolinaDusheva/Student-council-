// ============================================================
// ФАЙЛ: Program.cs
// ЦЕЛЬ: Това е входната точка на цялото ASP.NET Core приложение.
//       Тук се конфигурира и стартира уеб сървърът, регистрират
//       се всички услуги (services), настройва се базата данни и
//       се добавят началните данни (seed data).
//
// КАК РАБОТИ .NET: При стартиране средата изпълнява този файл.
//   Използва се "builder" шаблон — първо се описва КАК да се
//   построи приложението, след това се извиква Build() и накрая
//   Run() за да тръгне сървърът.
// ============================================================

using Microsoft.EntityFrameworkCore;
using StudentCouncil.Components;
using StudentCouncil.Data;

// WebApplication.CreateBuilder(args) създава "строител" (builder).
// Той събира всички настройки: конфигурационни файлове (appsettings.json),
// environment variables, command-line аргументи и т.н.
// "args" са аргументите от командния ред (обикновено празни при разработка).
var builder = WebApplication.CreateBuilder(args);

// ── Регистрация на Blazor ──────────────────────────────────────────────────
// AddRazorComponents() казва на ASP.NET да поддържа Razor Components (Blazor).
// AddInteractiveServerComponents() активира режима "InteractiveServer" —
// при него компонентите се изпълняват НА СЪРВЪРА и комуникират с браузъра
// чрез SignalR (постоянна WebSocket връзка). Промените в UI се изпращат
// в реално време без пълно презареждане на страницата.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Регистрация на базата данни (EF Core) ─────────────────────────────────
// AddDbContext<AppDbContext>() регистрира нашия контекст на базата данни
// в системата за Dependency Injection (DI). Когато даден компонент поиска
// AppDbContext чрез @inject, .NET автоматично създава инстанция и я предоставя.
//
// options.UseSqlite(...) — казваме на EF Core да използва SQLite като
// конкретна база данни. Може лесно да се смени с UseSqlServer() или UseNpgsql()
// за PostgreSQL без промяна в останалия код — това е силата на абстракцията.
//
// GetConnectionString("DefaultConnection") чете низа за връзка от
// appsettings.json, секция "ConnectionStrings": { "DefaultConnection": "..." }
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Build() финализира конфигурацията и създава обекта WebApplication.
// След това вече не можем да добавяме нови услуги.
var app = builder.Build();

// ── Инициализация на базата данни и начални данни (Seed) ──────────────────
// Тъй като AppDbContext е регистриран като "scoped" услуга (нов обект на
// всяка HTTP заявка), трябва ни временен обхват (scope), за да го достъпим
// извън обичайния цикъл на заявка. CreateScope() прави точно това.
// "using" гарантира, че scope-ът ще бъде освободен след блока — важно е
// за правилното управление на ресурсите.
using (var scope = app.Services.CreateScope())
{
    // GetRequiredService<AppDbContext>() взима нашия DbContext от DI контейнера.
    // Ако услугата не е регистрирана, хвърля изключение (за разлика от
    // GetService<>, което би върнало null).
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // EnsureCreated() проверява дали базата данни вече съществува.
    // Ако НЕ съществува — я създава автоматично според моделите в DbContext.
    // ВАЖНО: EnsureCreated() НЕ прилага миграции. При промяна на модела
    // трябва или да изтриеш базата и да я пресъздадеш, или да използваш
    // Migrate() заедно с EF Core миграции.
    db.Database.EnsureCreated();

    // Dictionary<string, string> е речник — колекция от двойки ключ-стойност.
    // Тук ключ = пълно име, стойност = длъжност.
    // Използваме го, за да зададем специфични длъжности на определени хора,
    // а всички останали ще получат "Член" по подразбиране.
    var correctPositions = new Dictionary<string, string>
    {
        ["Полина Душева"]       = "Председател",
        ["Петър Чернаев"]       = "Зам.-председател",
        ["Белослава Иванова"]   = "Секретар",
        ["Димитър Драгнев"]     = "Социална дейност",
        ["Светослав Иванов"]    = "Спорт",
    };

    // db.Members.Any() изпълнява SQL: SELECT CASE WHEN EXISTS(SELECT 1 FROM Members) ...
    // Ако таблицата е ПРАЗНА (нямаме нито един член), добавяме началните данни.
    // Условието !db.Members.Any() предпазва от дублиране при всяко рестартиране.
    if (!db.Members.Any())
    {
        // Масив от имена — var names = new[] { ... } е имплицитно типизиран масив.
        // C# разбира типа (string[]) от стойностите в него.
        var names = new[]
        {
            "Полина Душева", "Петър Чернаев", "Белослава Иванова",
            "Елени Ванева", "Борис Георгиев", "Борислав Атанасов",
            "Лора Узунова", "Светозар Вълев", "Диляра Мустафа",
            "Ебру Назми", "Боряна Георгиева", "Ивана Русева",
            "Мария Стоимирова", "Преслава Добрева", "Симеон Маринов",
            "Борис Соколов", "Венелин Узунов", "Тугай Ебадула",
            "Валентина Христова", "Николай Георгиев", "Никол Тошкова",
            "Антония Колева", "Дамян Караангелов",
            "Димитър Драгнев", "Светослав Иванов",
        };

        // За всяко име създаваме обект Member и го добавяме в DbSet.
        // TryGetValue проверява дали името е в речника — ако да, взима длъжността,
        // ако не — присвоява "Член". Тернарният оператор ?: е съкратен if-else.
        // db.Members.Add() НЕ записва веднага в базата — само следи промяната.
        foreach (var name in names)
        {
            var pos = correctPositions.TryGetValue(name, out var p) ? p : "Член";
            db.Members.Add(new StudentCouncil.Models.Member { Name = name, Position = pos });
        }

        // SaveChanges() ЗАПИСВА всички натрупани промени в базата данни с
        // един SQL transaction. Извиква се СЛЕД всички Add/Remove/Update,
        // не след всяка поотделно — по-ефективно е.
        db.SaveChanges();
    }
    else
    {
        // Ако базата вече има членове, проверяваме само дали длъжностите
        // на ключовите хора са правилни. Ако не са — поправяме ги.
        // "changed" е флаг, за да не извикваме SaveChanges() без нужда.
        bool changed = false;

        // foreach може да деконструира двойки (tuple) от Dictionary
        // директно в (name, position) — синтактична захар на C# 7+.
        foreach (var (name, position) in correctPositions)
        {
            // FirstOrDefault връща първия намерен обект или null.
            // Lambda x => x.Name == name е условието за търсене
            // (като WHERE клауза в SQL).
            var m = db.Members.FirstOrDefault(x => x.Name == name);
            if (m != null && m.Position != position)
            {
                // EF Core следи промените по обектите (change tracking).
                // Простото присвояване m.Position = position е достатъчно —
                // EF ще генерира UPDATE заявка при SaveChanges().
                m.Position = position;
                changed = true;
            }
        }

        // Записваме само ако наистина е имало промяна — пестим I/O.
        if (changed) db.SaveChanges();
    }
}

// ── Конфигурация на HTTP pipeline ─────────────────────────────────────────
// Всяка HTTP заявка минава последователно през "middleware" компоненти.
// Редът на регистрация е важен!

// В production режим показваме специална страница при грешки,
// вместо техническите подробности (опасно е да се показват на потребители).
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS = HTTP Strict Transport Security — казва на браузъра да ползва
    // само HTTPS за следващите заявки. Само за production.
    app.UseHsts();
}

// При 404, 500 и др. статус кодове пренасочва към /not-found страница.
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Автоматично пренасочва HTTP заявки към HTTPS за сигурност.
app.UseHttpsRedirection();

// Защита срещу CSRF (Cross-Site Request Forgery) атаки — задължително за
// интерактивни Blazor компоненти с форми.
app.UseAntiforgery();

// Обслужва статичните файлове (CSS, JS, изображения) от wwwroot папката.
app.MapStaticAssets();

// Регистрира всички Razor компоненти за маршрутизация (routing).
// App е коренният компонент (App.razor), от него започва дървото на UI.
// AddInteractiveServerRenderMode() активира двупосочната SignalR комуникация.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Стартира уеб сървъра и блокира изпълнението докато приложението работи.
// Без app.Run() програмата ще завърши веднага след стартиране.
app.Run();
