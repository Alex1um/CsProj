namespace HackatonService;

using System.Collections.ObjectModel;
using HackatonService.Participants;

public class PreferencesStore<T, V> : ReadOnlyDictionary<T, List<V>> where T : notnull where V : Participant
{

    public PreferencesStore(List<T> t, List<V> v) : base(t.Count == v.Count ? t.ToDictionary(x => x, x => Participant.CreateList(v)) : throw new Exception("Lists must have same length")) { }

    public PreferencesStore(Dictionary<T, List<V>> dict) : base(dict) { }

}