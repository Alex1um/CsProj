namespace HackatonService;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HackatonService.Participants;
using System.Linq;


public class Assignment<K, V> {
    public K Teamlead { get; set; }
    public V Junior { get; set; }
}

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
                resultDict[new Assignment<K, V> {Teamlead = teamlead, Junior = junior}] = teamLeadScore + juniorScore;
            }
        }
        return resultDict;
    }

    public static implicit operator Assignment<K, V>[](AssignmentStore<K, V> store) {
        var result = new Assignment<K, V>[store.Count];
        
        int index = 0;
        foreach (var (key, value) in store) {
            result[index] = new Assignment<K, V> {Teamlead = key, Junior = value};
            index += 1;
        }

        return result;
    }
}