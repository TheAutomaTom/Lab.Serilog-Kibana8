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
