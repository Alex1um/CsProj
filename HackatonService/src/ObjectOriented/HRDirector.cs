namespace HackatonService.src.ObjectOriented;

using HackatonService.src.ObjectOriented.Participants;
using ParticipantAssignment = (Participants.Teamlead, Participants.Junior);

class HRDirector
{

    private Dictionary<ParticipantAssignment, int> CalcSatisfactionIndex(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        var result_dict = new Dictionary<ParticipantAssignment, int>();
        foreach (var (teamlead, junior) in resultList)
        {
            var team_lead_index = junLists[junior].IndexOf(teamlead);
            var team_lead_score = teamleadLists.Count - team_lead_index;
            var junior_index = teamleadLists[teamlead].IndexOf(junior);
            var junior_score = junLists.Count - junior_index;
            result_dict[(teamlead, junior)] = team_lead_score + junior_score;
        }
        return result_dict;
    }

    private double GetHarmonicMean(Dictionary<ParticipantAssignment, int> resultDict)
    {
        var sum = 0.0;
        foreach (var value in resultDict.Values)
        {
            sum += 1.0 / value;
        }
        return resultDict.Count / sum;
    }

    public double CalculateHarmonicMean(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> resultList
    )
    {
        return GetHarmonicMean(CalcSatisfactionIndex(teamleadLists, junLists, resultList));
    }
}