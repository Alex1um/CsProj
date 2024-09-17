namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;

class HRManager<T> where T: ITeamBuildingStrategy
{

    public Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
    )
    {
        return T.BuildTeams(teamleads, juniors, teamleadLists, junLists);
    }
}