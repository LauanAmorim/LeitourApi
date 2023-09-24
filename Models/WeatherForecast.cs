using System.ComponentModel.DataAnnotations;

namespace LeitourApi.Models;

public class WeatherForecast
{
    [Key]
    public int id; 
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
