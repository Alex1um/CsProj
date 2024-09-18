using CsProj.src.ObjectOriented;
using Microsoft.Extensions.Hosting;

class HackatonWorker(IHostApplicationLifetime appLifetime, Hackaton hackaton, HRManager manager, HRDirector director) : IHostedService
{

    private readonly Hackaton _hackaton = hackaton;
    private readonly HRManager _manager = manager;
    private readonly HRDirector _director = director;

    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            var meanHarmonicsSum = _hackaton.Run(_manager, _director);
            Console.WriteLine("Mean Harmonic: " + meanHarmonicsSum);

            _appLifetime.StopApplication();
        });
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}