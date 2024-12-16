namespace HackatonBase.Participants;
using HackatonBase.Extensions;

public class Participant
{
    public required int Id { get; set; }
    public required string Name { get; set; }

    public static List<T> CreateList<T>(List<T> otherList) where T : Participant
    {
        return otherList.GetShuffled();
    }

    public override string ToString()
    {
        return $"{this.GetType().Name}({Id}, {Name})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Participant other)
        {
            return this.Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}