namespace HackatonService;

using HackatonService.Participants;
using HackatonService.Settings;
using HackatonService.DataIO;
using Microsoft.Extensions.Options;

class Hackaton(IOptions<DataSourceSettings> sourcesSettings)
{
    private readonly List<Teamlead> Teamleads = CSVReader.Read<Teamlead>(sourcesSettings.Value.TeamleadsListPath);
    private readonly List<Junior> Juniors = CSVReader.Read<Junior>(sourcesSettings.Value.JuniorsListPath);

    public double Run(HRManager manager, HRDirector director) {
        var junLists = new PreferencesStore<Junior, Teamlead>(Juniors, Teamleads);
        var teamleadLists = new PreferencesStore<Teamlead, Junior>(Teamleads, Juniors);
        var result_dict = manager.BuildTeams(Teamleads, Juniors, teamleadLists, junLists);
        return director.CalculateHarmonicMean(teamleadLists, junLists, result_dict);
    }

}