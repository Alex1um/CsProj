using HackatonBase.Strategies;
using Microsoft.Extensions.Options;
using HackatonBase.Participants;
using HackatonBase.Models;
using HackatonBase.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<HackatonSettings>().Bind(builder.Configuration);

builder.Services.AddTransient<ITeamBuildingStrategy, StableMarriageTeamBuildingStrategy>();
builder.Services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
builder.Services.AddSingleton<HRManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", (IOptions<HackatonSettings> settings, HRManager hrManager) =>
{
    return $"{hrManager.RequestsCounter} >= {settings.Value.RequestsThreshold}";
});

app.MapPost("/hackaton", async ([FromServices] IOptions<HackatonSettings> settings, [FromServices] HRManager hrManager, [FromBody] HackatonParticipantRegistration registration) =>
{
    hrManager.RequestsCounter += 1;

    if (registration.PatricipantType == "junior")
    {
        hrManager.AddParticipant(registration.ParticipantInfo.ToSub<Junior>(), registration.Preferences.ToSub<Teamlead>());
    }
    else if (registration.PatricipantType == "teamlead")
    {
        hrManager.AddParticipant(registration.ParticipantInfo.ToSub<Teamlead>(), registration.Preferences.ToSub<Junior>());
    }
    if (hrManager.RequestsCounter >= settings.Value.RequestsThreshold)
    {
        var (AssignmentStore, teamleadLists, junLists) = hrManager.GetBuildedTeams();
        var TeamRegistration = new TeamRegistration
        {
            resultList = AssignmentStore,
            teamleadLists = teamleadLists,
            junLists = junLists
        };
        using HttpClient client = new()
        {
            BaseAddress = settings.Value.HRDirectorURL
        };
        var request = await client.PostAsJsonAsync("/hackaton", TeamRegistration);
    }
});

app.Run(Environment.GetEnvironmentVariable("URL") ?? "http://0.0.0.0:8080");
