namespace HackatonService;

using HackatonService.Participants;
using HackatonService.Settings;
using HackatonService.DataIO;
using Microsoft.Extensions.Options;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;

class Hackaton
{
    private int Id { get; set; } = 0;
   
    private readonly List<Teamlead> Teamleads;
    private readonly List<Junior> Juniors;
    
    private readonly HackatonDbContext _context;

    public Hackaton(IOptions<DataSourceSettings> sourcesSettings, HackatonDbContext context) { 
        _context = context;

        Teamleads = CSVReader.Read<Teamlead>(sourcesSettings.Value.TeamleadsListPath);
        Juniors = CSVReader.Read<Junior>(sourcesSettings.Value.JuniorsListPath);

        context.Teamleads.AddRange(Teamleads);
        context.Juniors.AddRange(Juniors);
        context.SaveChanges();
    }

    public double Run(HRManager manager, HRDirector director) {
        var junLists = new PreferencesStore<Junior, Teamlead>(Juniors, Teamleads);
        _context.JuniorLists.Add(junLists.ToPreferencesStoreScheme(this));
        var teamleadLists = new PreferencesStore<Teamlead, Junior>(Teamleads, Juniors);
        _context.TeamleadLists.Add(teamleadLists.ToPreferencesStoreScheme(this));
        _context.SaveChanges();
        var resultDict = manager.BuildTeams(Teamleads, Juniors, teamleadLists, junLists);
        return director.CalculateHarmonicMean(teamleadLists, junLists, resultDict);
    }

}