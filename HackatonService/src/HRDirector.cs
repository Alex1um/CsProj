namespace HackatonService;

using HackatonService.DB;
using HackatonService.Extensions;
using HackatonService.Participants;

public class HRDirector()
{
    

    public Dictionary<Assignment<Teamlead, Junior>, int> CalcSatisfactionIndex(
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        AssignmentStore<Teamlead, Junior> resultList
    )
    {
        return resultList.CalculateScores(teamleadLists, junLists);
    }

    public double GetHarmonicMean(Dictionary<Assignment<Teamlead, Junior>, int> resultDict)
    {
        var mean = resultDict.Values.ToList().GetHarmonicMean();
        return mean;
    }
    
    public double CalculateHarmonicMean(
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        AssignmentStore<Teamlead, Junior> resultList
    )
    {
        return GetHarmonicMean(CalcSatisfactionIndex(teamleadLists, junLists, resultList));
    }
}