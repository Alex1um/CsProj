namespace HackatonBase.Models;
using HackatonBase.Participants;

public class HackatonParticipantRegistration {

    public int HackatonRunId { get; set; }
    public List<Participant> Preferences { get; set; }

    public string PatricipantType { get; set; }

    public Participant ParticipantInfo { get; set; }
}