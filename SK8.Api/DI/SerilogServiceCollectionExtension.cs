using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Elk8.Lab.Api.DI
{
  public static class SerilogServiceCollectionExtension
  {
    public static void ConfigureLogging(this IServiceCollection services, IConfiguration config, string env)
    {
      // Setup Logger
      Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(
          new ElasticsearchSinkOptions(new Uri(config["Elastic:Url"]))
          {
            ModifyConnectionSettings = x => x.BasicAuthentication(config["Elastic:Username"], config["Elastic:Password"]),
            AutoRegisterTemplate = true,

            // TODO: Move to Appsettings
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{env}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}",
            NumberOfReplicas = 2,
            NumberOfShards = 1
          }
        )
        .Enrich.WithProperty("Environment", env)
        .ReadFrom.Configuration(config)
        .CreateLogger();

      var x = configureElasticSink(config, env);


    }

    static ElasticsearchSinkOptions configureElasticSink(IConfiguration config, string env)
    {
      var url = config.GetValue<string>("Elastic:Url");
      return new ElasticsearchSinkOptions(new Uri(url))
      {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{env}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}",
        
        // TODO: Move to Appsettings
        NumberOfReplicas = 1,
        NumberOfShards = 2
      };
    }


  }
}
