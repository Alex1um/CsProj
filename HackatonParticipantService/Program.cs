using HackatonParticipantService.Settings;
using HackatonBase.Models;
using HackatonBase.Participants;
using HackatonBase.Extensions;
using HackatonBase;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

// builder.Services.Configure<ParticipantConfiguration>(builder.Configuration);
builder.Services.AddSingleton<ParticipantConfiguration>();

var app = builder.Build();

await app.Services.GetRequiredService<ParticipantConfiguration>().InitAsync();

app.MapGet("/", (ParticipantConfiguration config) => config.Info.Name);

app.MapGet("/hackaton", async (ParticipantConfiguration config, HackatonAnnouncement<Participant> participants) => {
    var shuffledParticipants = participants.participants.GetShuffled();
    var hackatonParticipantRegistration = new HackatonParticipantRegistration {
        Preferences = shuffledParticipants,
        ParticipantInfo = config.Info,
        PatricipantType = config.ParticipantType ?? "junior"
    };
    using HttpClient client = new()
        {
            BaseAddress = config.HRManagerURL
        };
    var request = await client.PostAsJsonAsync("/hackaton", hackatonParticipantRegistration);
    return "Ok";
});

await app.RunAsync(Environment.GetEnvironmentVariable("URL") ?? "http://localhost:8080");
