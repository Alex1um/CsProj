namespace HackatonService;

using System.Runtime.CompilerServices;
using HackatonService.Participants;

public class PreferList<T, V> : Dictionary<T, List<V>> where T : notnull where V : Participant
{

    public PreferList(List<T> t, List<V> v) : base()
    {
        foreach (var item in t)
        {
            Add(item, Participant.CreateList(v));
        }
    }

    public PreferList(Dictionary<T, List<V>> dict) : base(dict) {}
    
}