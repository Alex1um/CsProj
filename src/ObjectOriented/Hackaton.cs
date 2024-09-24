namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;


class Hackaton(string teamleadPath, string juniorsPath)
{

    private readonly List<Teamlead> Teamleads = CSVReader.Read<Teamlead>(teamleadPath);
    private readonly List<Junior> Juniors = CSVReader.Read<Junior>(juniorsPath);
    // private readonly HRManager<RandomTeamBuildingStrategy> Manager = new();
    private readonly HRManager<StableMarriageTeamBuildingStrategy> Manager = new();
    private readonly HRDirector Director = new();

    public double Run() {
        var junLists = new PreferList<Junior, Teamlead>(this.Juniors, Teamleads);
        var teamleadLists = new PreferList<Teamlead, Junior>(Teamleads, Juniors);
        var result_dict = Manager.BuildTeams(Teamleads, Juniors, teamleadLists, junLists);
        return Director.CalculateHarmonicMean(teamleadLists, junLists, result_dict);
    }
}