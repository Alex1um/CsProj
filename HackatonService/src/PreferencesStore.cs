namespace HackatonService;

using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.CompilerServices;
using HackatonService.Participants;

public abstract class PreferencesStore<T, V> : FrozenDictionary<T, List<V>> where T : notnull where V : Participant { 
}

public static class PreferencesStore
{
    public static PreferencesStore<T, V> ToPreferencesStore<T, V>(this List<T> t, List<V> v) where T : notnull where V : Participant
    {
        var _Dict = new Dictionary<T, List<V>>();
        foreach (var item in t)
        {
            _Dict.Add(item, Participant.CreateList(v));
        }
        return (PreferencesStore<T, V>)_Dict.ToFrozenDictionary();
    }

    public static PreferencesStore<T, V> ToPreferencesStore<T, V>(this Dictionary<T, List<V>> dict) where T : notnull where V : Participant { return (PreferencesStore<T, V>)dict.ToFrozenDictionary(); }

}