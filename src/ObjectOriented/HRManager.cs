namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;
using Microsoft.VisualBasic;
using Nsu.HackathonProblem.Contracts;

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