using Microsoft.EntityFrameworkCore;
using StudentCouncil.Components;
using StudentCouncil.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Members.Any())
    {
        var members = new (string Name, string Position)[]
        {
            ("Полина Душева",       "Председател"),
            ("Петър Чернаев",       "Зам.-председател"),
            ("Белослава Иванова",   "Секретар"),
            ("Елени Ванева",        "Член"),
            ("Борис Георгиев",      "Член"),
            ("Борислав Атанасов",   "Член"),
            ("Лора Узунова",        "Член"),
            ("Светозар Вълев",      "Член"),
            ("Диляра Мустафа",      "Член"),
            ("Ебру Назми",          "Член"),
            ("Боряна Георгиева",    "Член"),
            ("Ивана Русева",        "Член"),
            ("Мария Стоимирова",    "Член"),
            ("Преслава Добрева",    "Член"),
            ("Симеон Маринов",      "Член"),
            ("Борис Соколов",       "Член"),
            ("Венелин Узунов",      "Член"),
            ("Тугай Ебадула",       "Член"),
            ("Валентина Христова",  "Член"),
            ("Николай Георгиев",    "Член"),
            ("Никол Тошкова",       "Член"),
            ("Антония Колева",      "Член"),
            ("Дамян Караангелов",   "Член"),
            ("Димитър Драгнев",     "Сътрудник"),
            ("Светослав Иванов",    "Сътрудник"),
        };
        foreach (var (name, position) in members)
            db.Members.Add(new StudentCouncil.Models.Member { Name = name, Position = position });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
