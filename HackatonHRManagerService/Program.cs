using HackatonBase.Strategies;
using Microsoft.Extensions.Options;
using HackatonBase.Participants;
using HackatonBase.Models;
using HackatonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackatonBase.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<HackatonSettings>().Bind(builder.Configuration);

builder.Services.AddTransient<ITeamBuildingStrategy, StableMarriageTeamBuildingStrategy>();
builder.Services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
builder.Services.AddHostedService<HackatonReadyCheckerService>();
builder.Services.AddHostedService<HRManagerRabbitMQListenService>();
builder.Services.AddSingleton<HRManager>();

builder.Services.AddDbContextPool<HackatonDbContext>(options =>
{
    // options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES_CONNECTION_STRING"));
    options.UseNpgsql(builder.Configuration.GetValue<string>("POSTGRES_CONNECTION_STRING"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", (IOptions<HackatonSettings> settings, HRManager hrManager) =>
{
    return "Hello World!";
});

app.MapPost("/hackaton", async ([FromServices] IOptions<HackatonSettings> settings, [FromServices] HRManager hrManager, [FromBody] HackatonParticipantRegistration registration) =>
{
    if (registration.PatricipantType == "junior")
    {
        hrManager.AddJuniorParticipant(registration.ParticipantInfo.Id, registration.Preferences.Select(p => p.Id).ToList(), registration.HackatonRunId);
    }
    else if (registration.PatricipantType == "teamlead")
    {
        hrManager.AddTeamleadParticipant(registration.ParticipantInfo.Id, registration.Preferences.Select(p => p.Id).ToList(), registration.HackatonRunId);
    }
});

app.Run(Environment.GetEnvironmentVariable("URL") ?? "http://0.0.0.0:8080");
