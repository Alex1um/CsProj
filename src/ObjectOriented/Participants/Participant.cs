namespace CsProj.src.ObjectOriented.Participants;
using CsProj.src.ObjectOriented.Extensions;
abstract class Participant()
{
    public int Id { get; set; }
    public string Name { get; set; }

    // public Participant(int id, string name) : this() {
    //     Id = id;
    //     Name = name;
    // }

    public static List<T> createList<T>(List<T> other_list) where T : Participant
    {
        return other_list.GetShuffled();
    }

    public override string ToString()
    {
        return $"{this.GetType().Name}({Id}, {Name})";
    }
}
