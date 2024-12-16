using HackatonBase.Extensions;
using HackatonBase.Models;
using HackatonBase.Participants;
using Microsoft.AspNetCore.Mvc;
using HackatonBase.DB;
using Microsoft.EntityFrameworkCore;
using System.Text;
using HackatonBase;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSingleton<ParticipantService>();

builder.Services.AddSingleton<HRDirectorDbService>();
builder.Services.AddSingleton<HRDirector>();

builder.Services.AddDbContextPool<HackatonDbContext>(options =>
{
    // options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES_CONNECTION_STRING"));
    options.UseNpgsql(builder.Configuration.GetValue<string>("POSTGRES_CONNECTION_STRING"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.GetRequiredService<ParticipantService>().InitAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", (IConfiguration configuration, [FromServices] ParticipantService participantService) =>
{
    var stringBuilder = new StringBuilder();
    foreach (var uri in participantService.ParticipantsUri)
    {
        stringBuilder.AppendLine(uri.ToString());
    }
    return stringBuilder.ToString();
});

app.MapGet("/start", async (ParticipantService participantService, HRDirectorDbService dbService) =>
{
    var runId = dbService.CreateRun();
    var juniorParticipantsModel = new HackatonAnnouncement<Junior> { Participants = participantService.Juniors, HackatonRunId = runId };
    var teamleadsParticipantsModel = new HackatonAnnouncement<Teamlead> { Participants = participantService.Teamleads, HackatonRunId = runId };
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

app.MapPost("/hackaton", ([FromServices] HRDirector directorService, [FromBody] TeamRegistration teamRegistration, [FromServices] HRDirectorDbService dbService) =>
{
    var scores = directorService.CalcSatisfactionIndex(
        PreferencesStore<Teamlead, Junior>.FromJson(teamRegistration.teamleadLists),
        PreferencesStore<Junior, Teamlead>.FromJson(teamRegistration.junLists),
        AssignmentStore<Teamlead, Junior>.FromJson(teamRegistration.resultList)
    );
    var result = directorService.GetHarmonicMean(scores);
    dbService.AddMeanDataToDb(teamRegistration.HackatonRunId, scores, result);
    Console.WriteLine(result);
});


app.Run(Environment.GetEnvironmentVariable("URL") ?? "http://0.0.0.0:8080");
