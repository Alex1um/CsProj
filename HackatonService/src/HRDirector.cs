namespace HackatonService;

using HackatonService.DB;
using HackatonService.Extensions;
using HackatonService.Participants;

public class HRDirector(HackatonDbContext context)
{
    
    private readonly HackatonDbContext _context = context;

    public Dictionary<Assignment<Teamlead, Junior>, int> CalcSatisfactionIndex(
        int runId,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        AssignmentStore<Teamlead, Junior> resultList
    )
    {
        var scores = resultList.CalculateScores(teamleadLists, junLists);
        foreach (var (assignment, score) in scores)
        {
            var team = _context.Teams.Single(team => team.HackatonRunId == runId && team.TeamleadId == assignment.Teamlead.Id && team.JuniorId == assignment.Junior.Id);
            team.Score = score;
        }
        _context.SaveChanges();
        return scores;
    }

    public double GetHarmonicMean(int runId, Dictionary<Assignment<Teamlead, Junior>, int> resultDict)
    {
        var mean = resultDict.Values.ToList().GetHarmonicMean();
        _context.HachatonRuns.Single(run => run.Id == runId).mean = mean;
        _context.SaveChanges();
        return mean;
    }
    
    public double CalculateHarmonicMean(
        int runId,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        AssignmentStore<Teamlead, Junior> resultList
    )
    {
        return GetHarmonicMean(runId, CalcSatisfactionIndex(runId, teamleadLists, junLists, resultList));
    }
}