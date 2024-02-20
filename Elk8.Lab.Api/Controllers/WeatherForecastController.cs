using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;

namespace Elk8.Lab.Api.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class WeatherForecastController : ControllerBase
  {
    readonly IConfiguration _config;
    readonly ElasticsearchClient _client;
    readonly string _defaultIndex = "my_index";
    readonly ILogger<WeatherForecastController> _logger;
    readonly Random _rando = new Random();

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
    {
      _config = config;
      _logger = logger;

      var url = config.GetValue<string>("Elastic:Url");
      var settings = new ElasticsearchClientSettings(new Uri(url));

      _client = new ElasticsearchClient(settings);


    }

    [HttpPost]
    public async Task<IActionResult> CreateRando()
    {
      var wf = new WeatherForecast()
      {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(_rando.Next(365))),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = _summaries[Random.Shared.Next(_summaries.Length)],
      };

      var response = await _client.IndexAsync(wf, _defaultIndex);

      if (response.IsValidResponse)
      {
        return Ok(response.Result);
      }
      return BadRequest(response.Result);

    }








    static readonly string[] _summaries = new[]
    {
          "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
  }
}
