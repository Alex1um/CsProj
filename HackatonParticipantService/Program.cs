using HackatonParticipantService.Settings;
using HackatonBase.Models;
using HackatonBase.Participants;
using HackatonBase.Extensions;
using HackatonBase;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

// builder.Services.Configure<ParticipantConfiguration>(builder.Configuration);
builder.Services.AddSingleton<ParticipantConfiguration>();

builder.Services.AddHostedService<ParticipantRabbitMQListenService>();

var app = builder.Build();

await app.Services.GetRequiredService<ParticipantConfiguration>().InitAsync();

app.MapGet("/", (ParticipantConfiguration config) => config.Info.Name);

app.MapPost("/hackaton", async ([FromServices]ParticipantConfiguration config, [FromBody]HackatonAnnouncement<Participant> participants) => {
    var shuffledParticipants = participants.Participants.GetShuffled();
    var hackatonParticipantRegistration = new HackatonParticipantRegistration {
        HackatonRunId = participants.HackatonRunId,
        Preferences = shuffledParticipants,
        PatricipantType = config.ParticipantType ?? "junior",
        ParticipantInfo = config.Info,
    };
    using HttpClient client = new()
        {
            BaseAddress = config.HRManagerURL
        };
    var request = await client.PostAsJsonAsync("/hackaton", hackatonParticipantRegistration);
    return "Ok";
});

app.Run(Environment.GetEnvironmentVariable("URL") ?? "http://0.0.0.0:8080");
