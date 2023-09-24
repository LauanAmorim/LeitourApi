using LeitourApi.Models;
using LeitourApi.Repository;

namespace LeitourApi.Interfaces
{
    public interface IWeatherRepository : IGenericRepository<WeatherForecast>
    {
        Task<List<WeatherForecast>> GetWeatherAll();

        // Implements methods here
    }
}