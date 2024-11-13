namespace HackatonService;

using HackatonService.Participants;
using HackatonService.Settings;
using HackatonService.DataIO;
using Microsoft.Extensions.Options;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;

class Hackaton
{
    private int Id;
    private readonly List<Teamlead> Teamleads;
    private readonly List<Junior> Juniors;
    
    private readonly HackatonDbContext _context;

    public Hackaton(IOptions<DataSourceSettings> sourcesSettings, HackatonDbContext context) { 
        _context = context;

        Teamleads = CSVReader.Read<Teamlead>(sourcesSettings.Value.TeamleadsListPath);
        Juniors = CSVReader.Read<Junior>(sourcesSettings.Value.JuniorsListPath);

        var hackaton = new HackatonScheme();
        _context.Hachatons.Add(hackaton);

        _context.Database.EnsureCreated();
        foreach (var teamlead in Teamleads) {
            if (!_context.Teamleads.Contains(teamlead)) {
                _context.Teamleads.Add(teamlead);
            }
        }
        foreach (var junior in Juniors) {
            if (!_context.Juniors.Contains(junior)) {
                _context.Juniors.Add(junior);
            }
        }
        // _context.Teamleads.AddRange(Teamleads);
        // _context.Juniors.AddRange(Juniors);
        _context.SaveChanges();
        Id = hackaton.Id;
    }

    public double Run(HRManager manager, HRDirector director) {
        var run = new HackatonRunScheme();
        _context.Add(run);
        _context.SaveChanges();
        var junLists = new PreferencesStore<Junior, Teamlead>(Juniors, Teamleads);
        _context.JuniorLists.AddRange(junLists.ToPreferencsScheme(run));
        var teamleadLists = new PreferencesStore<Teamlead, Junior>(Teamleads, Juniors);
        _context.TeamleadLists.AddRange(teamleadLists.ToPreferencsScheme(run));
        _context.SaveChanges();
        var resultDict = manager.BuildTeams(run.Id, Teamleads, Juniors, teamleadLists, junLists);
        var harmonicMean = director.CalculateHarmonicMean(run.Id, teamleadLists, junLists, resultDict);
        return harmonicMean;
    }

}