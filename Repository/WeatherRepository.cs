using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using LeitourApi.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LeitourApi.Interfaces;

namespace LeitourApi.Repository
{
    public class WeatherRepository: GenericRepository<WeatherForecast>, IWeatherRepository
    {

        public WeatherRepository(LeitourContext context) : base(context)
        {
        }


        // Implements General Methods

        public Task<List<WeatherForecast>> GetWeatherAll(){
            return context.Set<WeatherForecast>().ToListAsync();
        }
    }
}