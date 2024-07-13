using CondigiBack.Contexts;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using System;
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

//Create a connection string to the database
var connectionString = Environment.GetEnvironmentVariable("PostgreDB");
//Register service to the connection

builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
