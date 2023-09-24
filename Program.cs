using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Repository;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

/*
builder.Services.AddDbContext<LeitourContext>(options =>
    options.UseMySqlServer(builder.Configuration.GetConnectionString("connection"))
    // Connection String is missing from configuration;
);*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
