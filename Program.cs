using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Repository;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<BookApiRepository>();

string? connection = "Server=database; port=3306; Database=db_leitour; Uid=mysql; Pwd=12345678";//builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connection);
builder.Services.AddDbContext<LeitourContext>(options =>
    options.UseMySql(connection,ServerVersion.AutoDetect(connection))
);


var app = builder.Build();


  app.UseSwagger();
  app.UseSwaggerUI();
// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
