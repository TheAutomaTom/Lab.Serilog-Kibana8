using System.Reflection;
using Elk8.Lab.Api.DI;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{env}.json", optional: true
  ).Build();




builder.Services.ConfigureLogging(config, env ??= "Local");

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


