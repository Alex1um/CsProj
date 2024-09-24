namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;


class Hackaton(string teamleadPath, string juniorsPath)
{

    private readonly List<Teamlead> Teamleads = CSVReader.Read<Teamlead>(teamleadPath);
    private readonly List<Junior> Juniors = CSVReader.Read<Junior>(juniorsPath);

    public double Run() {
        var junLists = new PreferList<Junior, Teamlead>(this.juniors, teamleads);
        var teamleadLists = new PreferList<Teamlead, Junior>(teamleads, juniors);
        var resultDict = manager.BuildTeams(teamleads, juniors, teamleadLists, junLists);
        return director.CalculateHarmonicMean(teamleadLists, junLists, resultDict);
    }
}