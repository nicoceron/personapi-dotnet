using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces; 
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Repositories;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// 1. Lee la cadena de conexión llamada "DefaultConnection" desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra PersonaDbContext para ser usado con SQL Server y la cadena de conexión leída
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IProfesionRepository, ProfesionRepository>();
builder.Services.AddScoped<ITelefonoRepository, TelefonoRepository>();
builder.Services.AddScoped<IEstudioRepository, EstudioRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





