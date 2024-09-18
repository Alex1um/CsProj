namespace CsProj.HackatonApp.ObjectOriented.Participants;

abstract public class Participant()
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static List<T> createList<T>(List<T> other_list) where T : Participant
    {
        return other_list.GetShuffled();
    }

    public override string ToString()
    {
        return $"{this.GetType().Name}({Id}, {Name})";
    }
}
