using HackatonBase.Extensions;
using HackatonBase.Models;
using HackatonBase.Participants;
using Microsoft.AspNetCore.Mvc;
using HackatonBase.DB;
using Microsoft.EntityFrameworkCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHostedService<ParticipantService>();
builder.Services.AddSingleton<HRDirectorDbService>();
builder.Services.AddSingleton<HRDirector>();

builder.Services.AddDbContextPool<HackatonDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES_CONNECTION_STRING"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", ([FromServices]ParticipantService participantService) =>
{
    var stringBuilder = new StringBuilder();
    foreach (var uri in participantService.ParticipantsUri)
    {
        stringBuilder.AppendLine(uri.ToString());
    }
    return stringBuilder.ToString();
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

app.MapPost("/hackaton", (HRDirector directorService, TeamRegistration teamRegistration, HRDirectorDbService dbService) =>
{
    var scores = directorService.CalcSatisfactionIndex(teamRegistration.teamleadLists, teamRegistration.junLists, teamRegistration.resultList);
    var result = directorService.GetHarmonicMean(scores);
    dbService.AddMeanDataToDb(teamRegistration.junLists, teamRegistration.teamleadLists, teamRegistration.resultList, scores, result);
    Console.WriteLine(result);
});


app.Run();
