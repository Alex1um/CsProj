namespace HackatonService;

using HackatonService.DB;
using HackatonService.Participants;

public class HRManager(ITeamBuildingStrategy strategy, HackatonDbContext context) 
{

    private readonly ITeamBuildingStrategy strategy = strategy;

    private readonly HackatonDbContext _context = context;

    public AssignmentStore<Teamlead, Junior> BuildTeams(
        int runId,
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        var teams = strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
        _context.Teams.AddRange(teams.ToTeamsScheme(runId));
        _context.SaveChanges();
        return teams;
    }
}