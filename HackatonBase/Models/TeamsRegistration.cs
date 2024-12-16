namespace HackatonBase.Models;
using HackatonBase.Participants;

public class TeamRegistration {
    public int HackatonRunId { get; init; }
    public List<Tuple<Teamlead, List<Junior>>> teamleadLists { get; init; }
    public List<Tuple<Junior, List<Teamlead>>> junLists { get; init; }
    public List<Tuple<Teamlead, Junior>> resultList { get; init; }
}