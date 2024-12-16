namespace HackatonBase.Models;
using HackatonBase.Participants;


public class HackatonAnnouncement<T> where T : Participant
{
    public int HackatonRunId;
    public List<T> Participants;
}
