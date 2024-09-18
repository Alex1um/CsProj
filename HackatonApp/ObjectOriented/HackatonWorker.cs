using CsProj.HackatonApp.ObjectOriented;
using Microsoft.Extensions.Hosting;
using CsProj.HackatonApp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

class HackatonWorker(ILogger<HackatonWorker> logger, IHostApplicationLifetime appLifetime, IOptions<HackatonSettings> appConfig, Hackaton hackaton, HRManager manager, HRDirector director) : IHostedService
{

    private readonly ILogger _logger = logger;
    private readonly IHostApplicationLifetime _appLifetime = appLifetime;
    private readonly IOptions<HackatonSettings> _appConfig = appConfig;
    private readonly Hackaton _hackaton = hackaton;
    private readonly HRManager _manager = manager;
    private readonly HRDirector _director = director;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {

            var meanHarmonicsSum = 0.0;
            for (int i = 0; i < _appConfig.Value.RunsCount; i++)
            {
                var meanHarmonics = _hackaton.Run(_manager, _director);
                _logger.LogInformation($"Mean Harmonic for iteration {i}: {meanHarmonics}");
                meanHarmonicsSum += meanHarmonics;
            }
            Console.WriteLine("Mean Harmonic: " + meanHarmonicsSum / _appConfig.Value.RunsCount);

            _appLifetime.StopApplication();
        });
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}