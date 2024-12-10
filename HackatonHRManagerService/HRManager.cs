using HackatonBase.Participants;
using HackatonBase.Strategies;
using HackatonBase;

public class HRManager(ITeamBuildingStrategy strategy)
{

    private readonly ITeamBuildingStrategy strategy = strategy;

    public AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}