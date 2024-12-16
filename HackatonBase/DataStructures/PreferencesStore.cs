namespace HackatonBase;

using System.Collections.ObjectModel;
using System.Text;
using HackatonBase.Participants;

public class PreferencesStore<T, V> : ReadOnlyDictionary<T, List<V>> where T : notnull, Participant where V : Participant
{

    public PreferencesStore(List<T> t, List<V> v) : base(t.Count == v.Count ? t.ToDictionary(x => x, x => Participant.CreateList(v)) : throw new Exception("Lists must have same length")) { }

    public PreferencesStore(Dictionary<T, List<V>> dict) : base(dict) { }


    public void Print()
    {
        var sb = new StringBuilder("{\n");
        foreach (var (key, value) in this)
        {
            sb.Append($"\t{key}: {value}\n");
        }
        sb.Append('}');
        Console.WriteLine(sb.ToString());
    }


    public List<Tuple<T, List<V>>> ToJsonSerializable()
    {
        return this.Select(x => Tuple.Create(x.Key, x.Value)).ToList();
    }

    public static PreferencesStore<T, V> FromJson(List<Tuple<T, List<V>>> lst) {
        return new PreferencesStore<T, V>(lst.ToDictionary(x => x.Item1, x => x.Item2));
    }
}
