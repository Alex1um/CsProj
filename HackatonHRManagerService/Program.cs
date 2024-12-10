using HackatonBase.Strategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddTransient<ITeamBuildingStrategy, StableMarriageTeamBuildingStrategy>();
builder.Services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
builder.Services.AddTransient<HRManager>();

builder.Services.AddOptions<HackatonSettings>().Bind(builder.Configuration);

var app = builder.Build();

app.MapGet("/", (IOptions<HackatonSettings> settings) => {
    return $"{settings.Value.RequestsCounter} >= {settings.Value.RequestsThreshold}";
});

app.MapPost("/hackaton", (IOptions<HackatonSettings> settings) => {
    settings.Value.RequestsCounter += 1;
    if (settings.Value.RequestsCounter >= settings.Value.RequestsThreshold) {
        
    }
});

app.Run();
