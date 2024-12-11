namespace HackatonBase.Models;
using HackatonBase.Participants;

public class TeamRegistration {
    public PreferencesStore<Teamlead, Junior> teamleadLists { get; init; }
    public PreferencesStore<Junior, Teamlead> junLists { get; init; }
    public AssignmentStore<Teamlead, Junior> resultList { get; init; }
}