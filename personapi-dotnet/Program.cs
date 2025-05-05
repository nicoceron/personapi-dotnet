using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces; 
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Repositories;
using personapi_dotnet.Models; // Add this using statement for DataSeeder



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// 1. Lee la cadena de conexi�n llamada "DefaultConnection" desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra PersonaDbContext para ser usado con SQL Server y la cadena de conexi�n le�da
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

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PersonaDbContext>();
        context.Database.Migrate();

        // Call the data seeder after migration
        DataSeeder.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        // Depending on the scenario, you might want to stop the application here
        // throw; 
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





