using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var Configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{environment}.json", optional: true
  ).Build();

// Setup Logger
Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .Enrich.WithExceptionDetails()
  .WriteTo.Debug()
  .WriteTo.Console()
  .WriteTo.Elasticsearch(
    new ElasticsearchSinkOptions(new Uri(Configuration["Elastic:Url"]))
    {
      AutoRegisterTemplate = true,
      IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{environment}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}",
      NumberOfReplicas = 2,
      NumberOfShards = 1
    }
  )
  .Enrich.WithProperty("Environment", environment)
  .ReadFrom.Configuration(Configuration)
  .CreateLogger();
builder.Host.UseSerilog();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
