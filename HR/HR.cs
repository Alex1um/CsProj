class HR
{

    public static Dictionary<(Teamlead, Junior), int> calcSatisfactionIndex(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> result_list
    )
    {
        var result_dict = new Dictionary<(Teamlead, Junior), int>();
        foreach (var (teamlead, junior) in result_list) {
            var team_lead_index = junLists[junior].IndexOf(teamlead);
            var team_lead_score = 20 - team_lead_index;
            var junior_index = teamleadLists[teamlead].IndexOf(junior);
            var junior_score = 20 - junior_index;
            result_dict[(teamlead, junior)] = team_lead_score + junior_score;
        }
        return result_dict;
    }

    public static double getHarmonicMean(Dictionary<(Teamlead, Junior), int> result_dict) {
        var sum = 0.0;
        foreach (var value in result_dict.Values) {
            sum += 1.0 / value;
        }
        return result_dict.Count / sum;
    }

    public static Dictionary<Teamlead, Junior> assignEmployees(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        )
    {
        var random = new Random();
        var juniors_shuffled = juniors.OrderBy(x => random.Next()).ToList();
        var teamleads_shuffled = teamleads.OrderBy(x => random.Next()).ToList();
        var result_list = new Dictionary<Teamlead, Junior>();
        for (int i = 0; i < juniors_shuffled.Count(); i++)
        {
            result_list.Add(teamleads_shuffled[i], juniors_shuffled[i]);
        }
        return result_list;
    }

}