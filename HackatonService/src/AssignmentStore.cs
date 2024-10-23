namespace HackatonService;
using System.Collections.ObjectModel;
using HackatonService.Participants;


public record Assignment<K, V>(
    K Teamlead,
    V Junior
);

public class AssignmentStore<K, V> : ReadOnlyDictionary<K, V> where K : notnull, Participant where V: notnull, Participant {

    public AssignmentStore(Dictionary<K, V> dict) : base(dict) { }
    
    public Dictionary<Assignment<K, V>, int>CalculateScores(
        PreferencesStore<K, V> kLists,
        PreferencesStore<V, K> vLists
    ) {
        var resultDict = new Dictionary<Assignment<K, V>, int>();
        foreach (var (teamlead, junior) in this)
        {
            var teamLeadIndex = vLists[junior].IndexOf(teamlead);
            var teamLeadScore = vLists[junior].Count - teamLeadIndex;
            var juniorIndex = kLists[teamlead].IndexOf(junior);
            var juniorScore = kLists[teamlead].Count - juniorIndex;
            if (teamlead is not null && junior is not null) {
                resultDict[new Assignment<K, V>(teamlead, junior)] = teamLeadScore + juniorScore;
            }
        }
        return resultDict;
    }
}