namespace HackatonService;

using HackatonService.DB;
using HackatonService.Participants;

public class HRManager(ITeamBuildingStrategy strategy, HackatonDbContext context) 
{

    private readonly HackatonDbContext _context = context;

    private readonly ITeamBuildingStrategy strategy = strategy;

    public AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        var teams = strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
        _context.Teams.AddRange(teams);
        _context.SaveChanges();
        return teams;
    }
}