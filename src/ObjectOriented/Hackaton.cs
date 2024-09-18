namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;


class Hackaton {

    private readonly List<Teamlead> teamleads;
    private readonly List<Junior> juniors;

    public Hackaton(String teamleadPath, String juniorsPath) {
        teamleads = CSVReader.Read<Teamlead>(teamleadPath);
        juniors = CSVReader.Read<Junior>(juniorsPath);
    }

    public double Run(HRManager manager, HRDirector director) {
        var junLists = new PreferList<Junior, Teamlead>(this.juniors, teamleads);
        var teamleadLists = new PreferList<Teamlead, Junior>(teamleads, juniors);
        var result_dict = manager.BuildTeams(teamleads, juniors, teamleadLists, junLists);
        return director.CalculateHarmonicMean(teamleadLists, junLists, result_dict);
    }
}