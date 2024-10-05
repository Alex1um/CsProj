namespace HackatonService;

using HackatonService.Participants;

public class HRManager(ITeamBuildingStrategy strategy) 
{

    private readonly ITeamBuildingStrategy strategy = strategy;

    public Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferList<Teamlead, Junior> teamleadLists,
        PreferList<Junior, Teamlead> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}