namespace HackatonBase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HackatonBase.Participants;
using System.Linq;
using Npgsql.Replication;
using System.Text;

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
        kLists.Print();
        vLists.Print();
        this.Print();
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

    public void Print() {
        var sb = new StringBuilder("{\n");
        foreach (var (key, value) in this)
        {
            sb.Append($"\t{key}: {value}\n");
        }
        sb.Append('}');
        Console.WriteLine(sb.ToString());
    }

    public List<Tuple<K, V>> ToJsonSerializable() {
        return this.Select(x => Tuple.Create(x.Key, x.Value)).ToList();
    }

    public static AssignmentStore<K, V> FromJson(List<Tuple<K, V>> lst) {
        return new AssignmentStore<K, V>(lst.ToDictionary(x => x.Item1, x => x.Item2));
    }
}
