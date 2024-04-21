using Elasticsearch.Net;
using Elk8.Lab.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elk8.Lab.Api.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class ElasticsearchClient8Controller : ControllerBase
  {
    readonly IConfiguration _config;
    readonly IElasticClient _client;
    readonly ILogger<ElasticsearchClient8Controller> _logger;

    readonly Random _rando = new Random();
    readonly ForecastSummary[] _summaries = Enum.GetValues(typeof(ForecastSummary)).Cast<ForecastSummary>().ToArray();

    public ElasticsearchClient8Controller(ILogger<ElasticsearchClient8Controller> logger, IConfiguration config, IElasticClient client)
    {
      _config = config;
      _logger = logger;
      _client = client;
    }

    WeatherForecast generateForecast()
    {
      var summary = (ForecastSummary)_summaries.GetValue(_rando.Next(_summaries.Length));
      var wf = new WeatherForecast()
      {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(_rando.Next(365))),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summary,
      };
      return wf;
    }


    [HttpPost]
    public async Task<IActionResult> CreateRando()
    {
      _logger.LogInformation("CreateRando() Begin");

      var example = generateForecast();

      var indexRequest = new IndexRequest<WeatherForecast>(example);

      Console.WriteLine($"Sending index request.\n Routing: {indexRequest.Routing}");
      var result = _client.Index(indexRequest);
      if(!result.IsValid)
      {
        _logger.LogError("CreateRando() Error: {Error}", result.ServerError.Error.Reason);
        return BadRequest(result.ServerError.Error.Reason);
      }

      if(result.Result != Result.Created)
      {
        _logger.LogError("CreateRando() Error: {Error}", result.Result);
        return BadRequest(result.Result);
      }


      return Ok(result.Result.ToString());

    }


    //async Task<IActionResult> processResult(ISearchResponse<WeatherForecast> response)
    //{
    //  var results = new List<WeatherForecast>();
    //  if (response != null && response.IsValid)
    //  {
    //    results = response.Documents.Select(doc =>
    //    {
    //      return new WeatherForecast()
    //      {
    //        Name = doc.Name,
    //        Description = doc.Description,
    //        OtherThing = doc.OtherThing
    //      };
    //    }).ToList();
    //    return Ok(results);
    //  }
    //  return BadRequest(results);

    //}


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var responses = _client.Search<WeatherForecast>(s =>
          s.Query(q => q
           .MatchAll()
          )
      );

      var results = new List<WeatherForecast>();
      foreach(var response in responses.Documents)
      {
        //var result = await processResult(response);
        results.Add(response);
      }
      return Ok(new { count = results.Count(), data = results });
    }



    [HttpGet]
    public async Task<IActionResult> IntentionallyThrow(string someParameter)
    {
      _logger.LogInformation("IntentionallyThrow() Begin");

      try
      {
        throw new Exception("IntentionallyThrow() Exception Message!");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "IntentionallyThrow() Exception", someParameter);
        return BadRequest(ex.Message);
      }

    }






  }
}
