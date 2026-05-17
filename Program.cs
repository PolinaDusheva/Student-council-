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

    var correctPositions = new Dictionary<string, string>
    {
        ["Полина Душева"]       = "Председател",
        ["Петър Чернаев"]       = "Зам.-председател",
        ["Белослава Иванова"]   = "Секретар",
        ["Димитър Драгнев"]     = "Сътрудник",
        ["Светослав Иванов"]    = "Сътрудник",
    };

    if (!db.Members.Any())
    {
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
        foreach (var name in names)
        {
            var pos = correctPositions.TryGetValue(name, out var p) ? p : "Член";
            db.Members.Add(new StudentCouncil.Models.Member { Name = name, Position = pos });
        }
        db.SaveChanges();
    }
    else
    {
        bool changed = false;
        foreach (var (name, position) in correctPositions)
        {
            var m = db.Members.FirstOrDefault(x => x.Name == name);
            if (m != null && m.Position != position)
            {
                m.Position = position;
                changed = true;
            }
        }
        if (changed) db.SaveChanges();
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
