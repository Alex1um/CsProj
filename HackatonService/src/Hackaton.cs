namespace HackatonService;

using HackatonService.Participants;
using HackatonService.Settings;
using HackatonService.DataIO;
using Microsoft.Extensions.Options;

class Hackaton
{
    private readonly List<Teamlead> Teamleads;
    private readonly List<Junior> Juniors;

    private readonly PreferencesStore<Junior, Teamlead> juniorsTeamleads;
    private readonly PreferencesStore<Teamlead, Junior> teamleadsJuniors;

    public Hackaton(IOptions<DataSourceSettings> sourcesSettings) {
        Teamleads = CSVReader.Read<Teamlead>(sourcesSettings.Value.TeamleadsListPath);
        Juniors = CSVReader.Read<Junior>(sourcesSettings.Value.JuniorsListPath);
        juniorsTeamleads = new PreferencesStore<Junior, Teamlead>(Juniors, Teamleads);
        teamleadsJuniors = new PreferencesStore<Teamlead, Junior>(Teamleads, Juniors);
    }

    public Hackaton(List<Teamlead> teamleads, List<Junior> juniors, PreferencesStore<Junior, Teamlead> juniorsTeamleads, PreferencesStore<Teamlead, Junior> teamleadsJuniors) {
        Teamleads = teamleads;
        Juniors = juniors;
        this.juniorsTeamleads = juniorsTeamleads;
        this.teamleadsJuniors = teamleadsJuniors;
    }

    public double Run(HRManager manager, HRDirector director) {
        var resultDict = manager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads);
        return director.CalculateHarmonicMean(teamleadsJuniors, juniorsTeamleads, resultDict);
    }

}