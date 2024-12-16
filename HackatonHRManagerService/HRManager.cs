using HackatonBase.Participants;
using HackatonBase.Strategies;
using HackatonBase.DB;
using HackatonBase;
using Microsoft.EntityFrameworkCore;

public class HRManager(ITeamBuildingStrategy strategy, HackatonDbContext context)
{
    private readonly HackatonDbContext _context = context;

    private readonly ITeamBuildingStrategy strategy = strategy;

    public void AddJuniorParticipant(int juniorId, List<int> teamleadPreferences, int HackatonRunId) {
        _context.JuniorLists.Add(new PreferenceScheme<Junior, Teamlead> {
            HackatonRunId = HackatonRunId,
            UnitId = juniorId,
            Prefered = teamleadPreferences
        });
        _context.SaveChanges();
    }

    public void AddTeamleadParticipant(int teamleadId, List<int> juniorPreferences, int HackatonRunId) {
        _context.TeamleadLists.Add(new PreferenceScheme<Teamlead, Junior> {
            HackatonRunId = HackatonRunId,
            UnitId = teamleadId,
            Prefered = juniorPreferences
        });
        _context.SaveChanges();
    }

    public AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}