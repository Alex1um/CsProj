public class ParticipantList(int owner)
{
    public int OwnerId { get; } = owner;

    public int[] Members { get; private set; } = new int[20];

}