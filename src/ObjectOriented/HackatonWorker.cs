using CsProj.src.ObjectOriented;
using Microsoft.Extensions.Hosting;
using CsProj.src;
using Microsoft.Extensions.Options;

class HackatonWorker(IHostApplicationLifetime appLifetime, IOptions<HackatonSettings> appConfig, Hackaton hackaton, HRManager manager, HRDirector director) : IHostedService
{

    private readonly Hackaton _hackaton = hackaton;
    private readonly HRManager _manager = manager;
    private readonly HRDirector _director = director;

    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    private readonly IOptions<HackatonSettings> _appConfig = appConfig;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {

            var meanHarmonicsSum = 0.0;
            for (int i = 0; i < _appConfig.Value.RunsCount; i++) {
                meanHarmonicsSum += _hackaton.Run(_manager, _director);
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