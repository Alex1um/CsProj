namespace HackatonService.src.ObjectOriented.Participants;

abstract public class Participant
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public static List<T> CreateList<T>(List<T> other_list) where T : Participant
    {
        return other_list.GetShuffled();
    }

    public override string ToString()
    {
        return $"{this.GetType().Name}({Id}, {Name})";
    }

}