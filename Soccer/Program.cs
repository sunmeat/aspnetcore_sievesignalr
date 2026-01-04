using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Soccer.Hubs;
using Soccer.Models;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SoccerContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

// додаємо SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Error404");

// додаємо ендпоінт для SignalR
app.MapHub<PlayersHub>("/playersHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Players}/{action=Index}/{id?}");

app.Run();

// для перевірки необхідно відрити декілька вкладок браузера з головною сторінкою додатку
// та спробувати додати, відредагувати або видалити гравця в одній з вкладок
// основні зміни було внесено в PlayersController.cs та Index.cshtml