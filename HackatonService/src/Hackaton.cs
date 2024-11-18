namespace HackatonService;

using HackatonService.Participants;
using HackatonService.Settings;
using HackatonService.DataIO;
using Microsoft.Extensions.Options;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;

class Hackaton
{
    public int Id;
    private readonly List<Teamlead> Teamleads;
    private readonly List<Junior> Juniors;

    private readonly PreferencesStore<Junior, Teamlead> juniorsTeamleads;
    private readonly PreferencesStore<Teamlead, Junior> teamleadsJuniors;

    private readonly HackatonDbContext _context;

    public Hackaton(IOptions<DataSourceSettings> sourcesSettings, HackatonDbContext context)
    {
        _context = context;

        Teamleads = CSVReader.Read<Teamlead>(sourcesSettings.Value.TeamleadsListPath);
        Juniors = CSVReader.Read<Junior>(sourcesSettings.Value.JuniorsListPath);


        juniorsTeamleads = new PreferencesStore<Junior, Teamlead>(Juniors, Teamleads);
        teamleadsJuniors = new PreferencesStore<Teamlead, Junior>(Teamleads, Juniors);

        AddDataToDb();
    }

    private void AddDataToDb()
    {
        _context.Database.EnsureCreated();

        var hackaton = new HackatonScheme();
        _context.Hachatons.Add(hackaton);
        _context.SaveChanges();
        Id = hackaton.Id;

        foreach (var teamlead in Teamleads)
        {
            if (!_context.Teamleads.Contains(teamlead))
            {
                _context.Teamleads.Add(teamlead);
            }
        }
        foreach (var junior in Juniors)
        {
            if (!_context.Juniors.Contains(junior))
            {
                _context.Juniors.Add(junior);
            }
        }
        _context.SaveChanges();

        _context.JuniorLists.AddRange(juniorsTeamleads.ToPreferencsScheme(Id));
        _context.TeamleadLists.AddRange(teamleadsJuniors.ToPreferencsScheme(Id));
        _context.SaveChanges();
    }

    public Hackaton(List<Teamlead> teamleads, List<Junior> juniors, PreferencesStore<Junior, Teamlead> juniorsTeamleads, PreferencesStore<Teamlead, Junior> teamleadsJuniors, HackatonDbContext context)
    {
        _context = context;
        Teamleads = teamleads;
        Juniors = juniors;
        this.juniorsTeamleads = juniorsTeamleads;
        this.teamleadsJuniors = teamleadsJuniors;
        AddDataToDb();
    }

    public double Run(HRManager manager, HRDirector director)
    {
        var run = new HackatonRunScheme
        {
            HackatonId = Id
        };
        _context.Add(run);
        _context.SaveChanges();
        var teams = manager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads);
        _context.Teams.AddRange(teams.ToTeamsScheme(run.Id));
        _context.SaveChanges();
        var scores = director.CalcSatisfactionIndex(teamleadsJuniors, juniorsTeamleads, teams);
        foreach (var (assignment, score) in scores)
        {
            var team = _context.Teams.Single(team => team.HackatonRunId == run.Id && team.TeamleadId == assignment.Teamlead.Id && team.JuniorId == assignment.Junior.Id);
            team.Score = score;
        }
        _context.SaveChanges();
        var harmonicMean = director.GetHarmonicMean(scores);
        _context.HachatonRuns.Single(srun => srun.Id == run.Id).mean = harmonicMean;
        _context.SaveChanges();
        return harmonicMean;
    }

}