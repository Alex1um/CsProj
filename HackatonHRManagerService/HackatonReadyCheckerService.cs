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
        _timer = new Timer(RunReadyHackatons, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    async private void RunReadyHackatons(object? state)
    {
        Console.WriteLine(settings.HRDirectorURL);
        var notCalculatedIds = _context.HachatonRuns.Where(x => x.mean == null).Select(x => x.Id).ToHashSet();

        var teamleadCount = _context.Teamleads.Count();

        var teamleadListsIds = _context
            .TeamleadLists
            .Select(tl => new Tuple<int, List<int>>(tl.HackatonRunId, tl.Prefered.ToList()))
            .ToList()
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Count())
            .Where(pair => pair.Value == teamleadCount && notCalculatedIds.Contains(pair.Key))
            .Select(x => x.Key)
            .ToHashSet();

        var juniorCount = _context.Juniors.Count();
        var juniorListsIds = _context
            .JuniorLists
            .Select(tl => new Tuple<int, List<int>>(tl.HackatonRunId, tl.Prefered.ToList()))
            .ToList()
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Count())
            .Where(pair => pair.Value == juniorCount && notCalculatedIds.Contains(pair.Key))
            .Select(x => x.Key)
            .ToHashSet();

        var notFullTeamsIds = _context
            .Teams
            .Select(tl => new Tuple<int, int, int>(tl.HackatonRunId, tl.JuniorId, tl.TeamleadId))
            .ToList()
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Count())
            .Where(pair => pair.Value > 0)
            .Select(x => x.Key)
            .ToHashSet();
        
        var toRunIds = teamleadListsIds
            .Intersect(juniorListsIds)
            .ToHashSet();
        
        using HttpClient client = new()
        {
            BaseAddress = settings.HRDirectorURL
        };
        foreach (var runId in toRunIds) {
            Console.WriteLine(runId);
            List<Junior> juniors = [.. _context.Juniors];
            List<Teamlead> teamleads = [.. _context.Teamleads];
            PreferencesStore<Teamlead, Junior> teamleadLists = new (
                _context
                    .TeamleadLists
                    .Select(x => new Tuple<int, List<int>, int>(x.UnitId, x.Prefered.ToList(), x.HackatonRunId))
                    .ToList()
                    .Where(x => x.Item3 == runId)
                    .ToDictionary(x => x.Item1.ToParticipant(teamleads), x => x.Item2.ToParticipants(juniors))
                );
            PreferencesStore<Junior, Teamlead> juniorsTeamleads = new (
                _context
                    .JuniorLists
                    .Select(x => new Tuple<int, List<int>, int>(x.UnitId, x.Prefered.ToList(), x.HackatonRunId))
                    .ToList()
                    .Where(x => x.Item3 == runId)
                    .ToDictionary(x => x.Item1.ToParticipant(juniors), x => x.Item2.ToParticipants(teamleads))
                );
            var teams = hrManager.BuildTeams(teamleads, juniors, teamleadLists, juniorsTeamleads);
            if (!notFullTeamsIds.Contains(runId)) {
                _context.Teams.AddRange(teams.ToTeamsScheme(runId));
                _context.SaveChanges();
            }
            var TeamRegistration = new TeamRegistration
            {
                HackatonRunId = runId,
                resultList = teams.ToJsonSerializable(),
                teamleadLists = teamleadLists.ToJsonSerializable(),
                junLists = juniorsTeamleads.ToJsonSerializable()
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