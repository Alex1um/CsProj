using HackatonParticipantService.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HackatonBase.Models;
using HackatonBase.Participants;
using HackatonBase.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

// builder.Services.Configure<ParticipantConfiguration>(builder.Configuration);
builder.Services.AddHostedService<ParticipantConfiguration>();

var app = builder.Build();

app.MapGet("/", (ParticipantConfiguration config) => config.Name);

app.MapGet("/hackaton", async (ParticipantConfiguration config, HackatonAnnouncement<Participant> participants) => {
    var shuffled_participants = participants.participants.GetShuffled();
    var request = HttpClient.PostAsync(config.HRManagerURL + "/hackaton");
    return "Ok";
});

await app.RunAsync(Environment.GetEnvironmentVariable("URL") ?? "http://localhost:8080");
