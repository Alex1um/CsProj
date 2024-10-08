namespace HackatonService;

using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using HackatonService.Participants;

public class PreferencesStore<T, V> where T : notnull where V : Participant
{
    private Dictionary<T, List<V>> _Dict = new();

    public PreferencesStore(List<T> t, List<V> v) : base()
    {
        foreach (var item in t)
        {
            _Dict.Add(item, Participant.CreateList(v));
        }
    }

    public List<V> this[T key] { get => _Dict[key]; }

    public Dictionary<T, List<V>>.Enumerator GetEnumerator() {
        return _Dict.GetEnumerator();
    }

    public int Count { get => _Dict.Count; }

    public PreferencesStore(Dictionary<T, List<V>> dict) { _Dict = dict; }

}