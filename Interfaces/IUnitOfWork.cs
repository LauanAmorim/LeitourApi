namespace LeitourApi.Interfaces
{
    public interface IUnitOfWork
    {
        IWeatherRepository Weather {get;}

        int Complete();
    }
}