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
using CondigiBack.Modules.Contracts.Services;
using Microsoft.OpenApi.Models;
using CondigiBack.Libs.Services;
using CloudinaryDotNet;
using DinkToPdf.Contracts;
using DinkToPdf;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//Create a connection string to the database
var connectionString = Environment.GetEnvironmentVariable("PostgreDB");


//Register service to the connection
builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionString));

//Add Utils
builder.Services.AddSingleton<JWT>();
builder.Services.AddSingleton<Encrypt>();

// Generate and store the IV key at application startup
Encrypt.GenerateRandomIv();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedHosts",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

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
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY_JWT")))
        };
    }
);

var cloudinarySettings = new CloudinarySettings
{
    CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
    ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
    ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
};
var cloudinary = new Cloudinary(new Account(
    cloudinarySettings.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret
));

builder.Services.AddSingleton(cloudinary);

// Registrar servicios para la generación de PDFs y subida a Cloudinary
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider => new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<PdfGeneratorClass>();
builder.Services.AddSingleton<CloudinaryUploader>();

// Add services to the container.
builder.Services.AddScoped<GeographyService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<ContractTypeService>();
builder.Services.AddScoped<ContractParticipantService>();
builder.Services.AddScoped<ContractAIService>();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddHttpClient<EmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Condigi API", Version = "v1" });

    // Definir el esquema de seguridad para usar tokens Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, inserta el token con 'Bearer ' antes del token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // Aplicar el filtro de seguridad
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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