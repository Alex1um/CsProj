namespace HackatonService;

using HackatonService.Participants;

public class HRManager(ITeamBuildingStrategy strategy) 
{

    private readonly ITeamBuildingStrategy strategy = strategy;

    public Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadLists,
        PreferencesStore<Junior, Teamlead> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}