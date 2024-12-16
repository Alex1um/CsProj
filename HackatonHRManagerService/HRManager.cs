using HackatonBase.Participants;
using HackatonBase.Strategies;
using HackatonBase.DB;
using HackatonBase;
using Microsoft.EntityFrameworkCore;

public class HRManager(ITeamBuildingStrategy strategy, HackatonDbContext context)
{
    private readonly HackatonDbContext _context = context;

    private readonly ITeamBuildingStrategy strategy = strategy;

    public void AddParticipant(Junior participant, List<Teamlead> preferences, int HackatonRunId) {
        _context.Add(new PreferenceScheme<Junior, Teamlead> {
            HackatonRunId = HackatonRunId,
            UnitId = participant.Id,
            Prefered = preferences.Select(p => p.Id).ToList()
        });
        _context.SaveChanges();
    }

    public void AddParticipant(Teamlead participant, List<Junior> preferences, int HackatonRunId) {
        _context.Add(new PreferenceScheme<Teamlead, Junior> {
            HackatonRunId = HackatonRunId,
            UnitId = participant.Id,
            Prefered = preferences.Select(p => p.Id).ToList()
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