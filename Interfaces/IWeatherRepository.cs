using LeitourApi.Models;
using LeitourApi.Repository;

namespace LeitourApi.Interfaces
{
    public interface IWeatherRepository: IGenericRepository<WeatherForecast>
    {
        new Task<List<WeatherForecast>> GetAll();

        // Implements methods here
    }
}