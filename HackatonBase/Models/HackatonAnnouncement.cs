namespace HackatonBase.Models;
using HackatonBase.Participants;


public class HackatonAnnouncement<T> where T : Participant
{
    public List<T> participants;
}
