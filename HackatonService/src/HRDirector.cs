namespace HackatonService;

using HackatonService.Participants;
using ParticipantAssignment = (Participants.Teamlead, Participants.Junior);

public class HRDirector
{

    public Dictionary<ParticipantAssignment, int> CalcSatisfactionIndex(
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        var resultDict = new Dictionary<ParticipantAssignment, int>();
        foreach (var (teamlead, junior) in resultList)
        {
            var teamLeadIndex = junLists[junior].IndexOf(teamlead);
            var teamLeadScore = junLists[junior].Count - teamLeadIndex;
            var juniorIndex = teamleadLists[teamlead].IndexOf(junior);
            var juniorScore = teamleadLists[teamlead].Count - juniorIndex;
            resultDict[(teamlead, junior)] = teamLeadScore + juniorScore;
        }
        return resultDict;
    }

    public double GetHarmonicMean(Dictionary<ParticipantAssignment, int> resultDict)
    {
        return GetHarmonicMean(resultDict.Values.ToList());
    }
    
    public double GetHarmonicMean(List<int> values)
    {
        var sum = 0.0;
        foreach (var value in values)
        {
            sum += 1.0 / value;
        }
        return values.Count / sum;
    }

    public double CalculateHarmonicMean(
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        return GetHarmonicMean(CalcSatisfactionIndex(teamleadLists, junLists, resultList));
    }
}