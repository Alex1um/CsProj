using HackatonBase.Participants;
using HackatonBase.Strategies;
using HackatonBase;

public class HRManager(ITeamBuildingStrategy strategy)
{
    private readonly ITeamBuildingStrategy strategy = strategy;

    public int RequestsCounter { get; set; } = 0;

    public Dictionary<Teamlead, List<Junior>> TeamleadPreferencesLists { get; set; } = [];
    public Dictionary<Junior, List<Teamlead>> JunPreferencesLists { get; set; } = [];

    public void AddParticipant(Junior participant, List<Teamlead> preferences) {
        JunPreferencesLists.Add(participant, preferences);
    }

    public void AddParticipant(Teamlead participant, List<Junior> preferences) {
        TeamleadPreferencesLists.Add(participant, preferences);
    }

    public (AssignmentStore<Teamlead, Junior>, PreferencesStore<Teamlead, Junior>, PreferencesStore<Junior, Teamlead>) GetBuildedTeams() {
        var teamleads = TeamleadPreferencesLists.Keys.ToList();
        var juniors = JunPreferencesLists.Keys.ToList();
        var teamleadLists = new PreferencesStore<Teamlead, Junior>(TeamleadPreferencesLists);
        var junLists = new PreferencesStore<Junior, Teamlead>(JunPreferencesLists);
        return (BuildTeams(teamleads, juniors, teamleadLists, junLists), teamleadLists, junLists);
    }

    private AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}