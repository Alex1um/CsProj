using HackatonBase.DB; 
using HackatonBase.Models; 
using HackatonBase.Participants; 
using HackatonBase;
using Microsoft.Extensions.Options;


class HackatonReadyCheckerService(HRManager hrManager, HackatonDbContext context, IOptions<HackatonSettings> settings) : IHostedService, IDisposable
{
    private Timer? _timer = null;

    private readonly HRManager hrManager = hrManager;

    private readonly HackatonDbContext _context = context;
    
    private readonly HackatonSettings settings = settings.Value;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(RunReadyHackatons, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    async private void RunReadyHackatons(object? state)
    {
        var notCalculatedIds = _context.HachatonRuns.Where(x => x.mean == null).Select(x => x.Id).ToHashSet();

        var teamleadCount = _context.Teamleads.Count();
        var teamleadListsIds = _context
            .TeamleadLists
            .Where(x => notCalculatedIds.Contains(x.HackatonRunId))
            .GroupBy(x => x.HackatonRunId)
            .ToDictionary(x => x.Key, x => x.Count())
            .Where((runId, cnt) => cnt == teamleadCount)
            .Select(x => x.Key)
            .ToHashSet();

        var juniorCount = _context.Juniors.Count();
        var juniorListsIds = _context
            .JuniorLists
            .Where(x => notCalculatedIds.Contains(x.HackatonRunId))
            .GroupBy(x => x.HackatonRunId)
            .ToDictionary(x => x.Key, x => x.Count())
            .Where((runId, cnt) => cnt == juniorCount)
            .Select(x => x.Key)
            .ToHashSet();

        using HttpClient client = new()
        {
            BaseAddress = settings.HRDirectorURL
        };
        foreach (var runId in teamleadListsIds.Intersect(juniorListsIds)) {
            List<Junior> juniors = [.. _context.Juniors];
            List<Teamlead> teamleads = [.. _context.Teamleads];
            PreferencesStore<Teamlead, Junior> teamleadLists = _context
                .TeamleadLists
                .Where(x => x.HackatonRunId == runId)
                .ToArray()
                .ToPreferenceStore(teamleads, juniors);
            PreferencesStore<Junior, Teamlead> juniorsTeamleads = _context
                .JuniorLists
                .Where(x => x.HackatonRunId == runId)
                .ToArray()
                .ToPreferenceStore(juniors, teamleads);
            var teams = hrManager.BuildTeams(teamleads, juniors, teamleadLists, juniorsTeamleads);
            _context.Teams.AddRange(teams.ToTeamsScheme(runId));
            _context.SaveChanges();
            var TeamRegistration = new TeamRegistration
            {
                HackatonRunId = runId,
                resultList = teams,
                teamleadLists = teamleadLists,
                junLists = juniorsTeamleads
            };
            var request = await client.PostAsJsonAsync("/hackaton", TeamRegistration);
        }        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}