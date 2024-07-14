using CondigiBack.Contexts;
using CondigiBack.Libs.Middleware;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Geography.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

//Create a connection string to the database
var connectionString = Environment.GetEnvironmentVariable("PostgreDB");

//Register service to the connection
builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionString));

//Add Utils
builder.Services.AddSingleton<JWT>();
builder.Services.AddSingleton<Encrypt>();

//Configure JWT Authentication and Authorization
builder.Services.AddAuthentication(config =>
{
    config = new AuthenticationOptions()
    {
        DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme,
        DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
    };
}
).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "CondigiBack",
        ValidAudience = "CondigiBack",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY_JWT")))
    };
}
);

//Roles
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    config.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
});

// Add services to the container.
builder.Services.AddScoped<GeographyService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//Error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

//CORS
app.UseCors("AllowedHosts");

app.UseAuthorization();

app.MapControllers();

app.Run();
