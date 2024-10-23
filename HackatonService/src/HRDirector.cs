namespace HackatonService;

using HackatonService.Extensions;
using HackatonService.Participants;

public class HRDirector
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
        return resultDict.Values.ToList().GetHarmonicMean();
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