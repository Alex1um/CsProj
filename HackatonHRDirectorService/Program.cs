using HackatonBase.Extensions;
using HackatonBase.Models;
using HackatonBase.Participants;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHostedService<ParticipantService>();
builder.Services.AddSingleton<HRDirector>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", ([FromServices]ParticipantService participantService) =>
{
    return $"{participantService.ParticipantsUri}";
});

app.MapGet("/start", async ([FromServices]ParticipantService participantService) =>
{
    var juniorParticipantsModel = new HackatonAnnouncement<Junior> { participants = participantService.Juniors };
    var teamleadsParticipantsModel = new HackatonAnnouncement<Teamlead> { participants = participantService.Teamleads };
    foreach (var participantURL in participantService.ParticipantsUri)
    {
        using HttpClient client = new()
        {
            BaseAddress = participantURL
        };
        if (participantURL.ToString().Contains("junior"))
        {
            var request = await client.PostAsJsonAsync("/hackaton", teamleadsParticipantsModel);
        }
        else
        {
            var request = await client.PostAsJsonAsync("/hackaton", juniorParticipantsModel);
        }
    }
});

app.MapPost("/hackaton", (HRDirector directorService, TeamRegistration teamRegistration) =>
{
    var result = directorService.CalculateHarmonicMean(teamRegistration.teamleadLists, teamRegistration.junLists, teamRegistration.resultList);
    Console.WriteLine(result);
});


app.Run();
