using HackatonParticipantService.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

// builder.Services.Configure<ParticipantConfiguration>(builder.Configuration);
builder.Services.AddHostedService<ParticipantConfiguration>();

var app = builder.Build();

app.MapGet("/", (ParticipantConfiguration config) => config.Name);

await app.RunAsync(Environment.GetEnvironmentVariable("URL") ?? "http://localhost:8080");
