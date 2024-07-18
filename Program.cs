using CondigiBack.Contexts;
using CondigiBack.Libs.Middleware;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Modules.Auth.Services;
using CondigiBack.Modules.Companies.Services;
using CondigiBack.Modules.Geography.Services;
using CondigiBack.Modules.Users.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
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
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY_JWT")))
    };
}
);

// Add services to the container.
builder.Services.AddScoped<GeographyService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CompanyService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToList();
        
        var errorResponse = new BadRequestResponse<object>
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Error = "Se han encontrado errores de validación.",
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

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
