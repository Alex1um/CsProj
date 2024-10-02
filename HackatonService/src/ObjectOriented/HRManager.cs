namespace HackatonService.src.ObjectOriented;

using HackatonService.src.ObjectOriented.Participants;

class HRManager(ITeamBuildingStrategy strategy) 
{

    private readonly ITeamBuildingStrategy strategy = strategy;

    public Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
    )
    {
        return strategy.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}