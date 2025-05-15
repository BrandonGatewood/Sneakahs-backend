using DotNetEnv;
using Sneakahs.Persistence.Data;
using Sneakahs.Application.Interfaces;
using Sneakahs.Infrastructure.Services;
using Sneakahs.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

// initialize app builder
var builder = WebApplication.CreateBuilder(args);

// Load env file for database connection JWT
Env.Load();

var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var connectionString = Environment.GetEnvironmentVariable("DB_URL");

if(string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
{
    throw new ArgumentException("Issuer and Audience must be provided");
}

// Auto-generate jwt secret
var randomBytes = new byte[32];
using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(randomBytes);
}
var secret = Convert.ToBase64String(randomBytes);

// Configure Authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Checks token for expiration, issuer, audience and signing key
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

// Register Services and DBContext
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJwtService>(provider => new JwtService(secret, issuer, audience));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// builds the app from everything above
var app = builder.Build();

// Connect to database
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

// Configure middleware (runs for each http request)
app.UseRouting();           // Matches request to endpoints
app.UseAuthentication();    // Checks for valid JWT
app.UseAuthorization();     // Enforces access control rules
app.MapControllers();       // maps requests to controller methods

// Start handling requests
app.Run();