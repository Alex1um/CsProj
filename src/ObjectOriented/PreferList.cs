namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;

class PreferList<T, V> : Dictionary<T, List<V>> where T : notnull where V : Participant
{

    public PreferList(List<T> t, List<V> v) : base()
    {
        foreach (var item in t)
        {
            Add(item, Participant.createList(v));
        }
    }
}