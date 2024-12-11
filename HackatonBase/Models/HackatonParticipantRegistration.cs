namespace HackatonBase.Models;
using HackatonBase.Participants;

public class HackatonParticipantRegistration {

    public List<Participant> Preferences { get; set; }

    public string PatricipantType { get; set; }

    public Participant ParticipantInfo { get; set; }
}