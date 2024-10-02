namespace HackatonService.src.ObjectOriented;

using HackatonService.src.ObjectOriented.Participants;
using ParticipantAssignment = (Participants.Teamlead, Participants.Junior);

public class HRDirector
{

    public Dictionary<ParticipantAssignment, int> CalcSatisfactionIndex(
        PreferList<Teamlead, Junior> teamleadLists,
        PreferList<Junior, Teamlead> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        var result_dict = new Dictionary<ParticipantAssignment, int>();
        foreach (var (teamlead, junior) in resultList)
        {
            var team_lead_index = junLists[junior].IndexOf(teamlead);
            var team_lead_score = junLists[junior].Count - team_lead_index;
            var junior_index = teamleadLists[teamlead].IndexOf(junior);
            var junior_score = teamleadLists[teamlead].Count - junior_index;
            result_dict[(teamlead, junior)] = team_lead_score + junior_score;
        }
        return result_dict;
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
        PreferList<Teamlead, Junior> teamleadLists,
        PreferList<Junior, Teamlead> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        return GetHarmonicMean(CalcSatisfactionIndex(teamleadLists, junLists, resultList));
    }
}