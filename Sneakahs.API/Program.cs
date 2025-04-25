using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Sneakahs.Api.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Get connection string from env
var connectionString = Environment.GetEnvironmentVariable("DB_URL");

// Register ApplicationDbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add controllers or other services if needed
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("✅ Successfully connected to the database!");
        }
        else
        {
            Console.WriteLine("❌ Failed to connect to the database.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"🔥 Exception while connecting to the DB: {ex.Message}");
    }
}

// Configure middleware (minimal for now)
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();