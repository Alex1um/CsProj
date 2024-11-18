namespace HackatonService;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using HackatonService.Settings;
using HackatonService.TUI;

class HackatonWorker(
    ILogger<HackatonWorker> logger,
    IHostApplicationLifetime appLifetime,
    TerminalUserInterface tui
    // IOptions<HackatonSettings> appConfig,
    // Hackaton hackaton,
    // HRManager manager,
    // HRDirector director
    ) : IHostedService
{

    private readonly ILogger _logger = logger;
    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    private readonly TerminalUserInterface _tui = tui;
    // private readonly IOptions<HackatonSettings> _appConfig = appConfig;
    // private readonly Hackaton _hackaton = hackaton;
    // private readonly HRManager _manager = manager;
    // private readonly HRDirector _director = director;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            _tui.Run();

            // var meanHarmonicsSum = 0.0;
            // for (int i = 0; i < _appConfig.Value.RunsCount; i++)
            // {
            //     var meanHarmonics = _hackaton.Run(_manager, _director);
            //     _logger.LogInformation($"Mean Harmonic for iteration {i}: {meanHarmonics}");
            //     meanHarmonicsSum += meanHarmonics;
            // }
            // Console.WriteLine("Mean Harmonic: " + meanHarmonicsSum / _appConfig.Value.RunsCount);

            _appLifetime.StopApplication();
        });
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}