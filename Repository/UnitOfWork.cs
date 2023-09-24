using System;
using LeitourApi.Interfaces;
using LeitourApi.Models;

namespace LeitourApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeitourContext context;
        public IWeatherRepository WeatherRepository {get;}

        public UnitOfWork(LeitourContext context, IWeatherRepository WeatherRepository)
        {
            this.context = context;
            this.WeatherRepository = WeatherRepository;
        }

        IWeatherRepository IUnitOfWork.WeatherRepository => throw new NotImplementedException();

        public int Complete() => context.SaveChanges();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}