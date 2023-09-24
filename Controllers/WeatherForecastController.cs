using Microsoft.AspNetCore.Mvc;
using LeitourApi.Models;
using LeitourApi.Repository;
using LeitourApi.Interfaces;

namespace LeitourApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;

    public WeatherForecastController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;
    
    [HttpGet]
    public async Task<ActionResult<List<WeatherForecast>>> DetailsList()
    {
        List<WeatherForecast> Weather = await unitOfWork.Weather.GetAll();
        return Weather;
    }

    [HttpGet("Debug/{teste}")]
    public async Task<ActionResult<string>> Debug(string teste)
    {
        return unitOfWork.Weather.Debug(teste);
    }
}
