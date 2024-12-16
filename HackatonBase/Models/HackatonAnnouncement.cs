namespace HackatonBase.Models;
using HackatonBase.Participants;


public class HackatonAnnouncement<T> where T : Participant
{
    public int HackatonRunId { get; set; }
    public List<T> Participants { get; set; }
}
