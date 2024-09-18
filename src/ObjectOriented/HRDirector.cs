namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;
using Assignment = (Participants.Teamlead, Participants.Junior);

class HRDirector {  
    
    private static Dictionary<Assignment, int> CalcSatisfactionIndex(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> result_list
    )
    {
        var result_dict = new Dictionary<Assignment, int>();
        foreach (var (teamlead, junior) in result_list)
        {
            var team_lead_index = junLists[junior].IndexOf(teamlead);
            var team_lead_score = teamleadLists.Count - team_lead_index;
            var junior_index = teamleadLists[teamlead].IndexOf(junior);
            var junior_score = junLists.Count - junior_index;
            result_dict[(teamlead, junior)] = team_lead_score + junior_score;
        }
        return result_dict;
    }
    
    private static double GetHarmonicMean(Dictionary<Assignment, int> result_dict)
    {
        var sum = 0.0;
        foreach (var value in result_dict.Values)
        {
            sum += 1.0 / value;
        }
        return result_dict.Count / sum;
    }
    
    public double CalculateHarmonicMean(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> result_list
    )
    {
        return GetHarmonicMean(CalcSatisfactionIndex(teamleadLists, junLists, result_list));
    }
}