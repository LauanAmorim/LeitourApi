namespace LeitourApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IWeatherRepository WeatherRepository {get;}
        int Complete();
    }
}