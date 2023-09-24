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
    
    public async Task<ActionResult<List<WeatherForecast>>> DetailsList()
    {
        List<WeatherForecast> Weather = await unitOfWork.WeatherRepository.GetAll();
        return Weather;
    }

}
