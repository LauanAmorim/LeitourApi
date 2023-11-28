using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Repository;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<BookApiRepository>();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connection);

builder.Services.AddDbContext<LeitourContext>(options =>{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:44398/")
           .AllowAnyHeader()
           .AllowAnyMethod();
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
