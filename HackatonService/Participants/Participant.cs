namespace HackatonService.Participants;
using HackatonService.Extensions;

abstract public class Participant
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public static List<T> CreateList<T>(List<T> otherList) where T : Participant
    {
        return otherList.GetShuffled();
    }

    public override string ToString()
    {
        return $"{this.GetType().Name}({Id}, {Name})";
    }

}