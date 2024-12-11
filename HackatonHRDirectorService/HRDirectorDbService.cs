using HackatonBase;
using HackatonBase.DB;
using HackatonBase.Extensions;
using HackatonBase.Participants;

class HRDirectorDbService
{
    public int HackatonId;
    private readonly HackatonDbContext _context;

    public HRDirectorDbService(HackatonDbContext context, ParticipantService participantService)
    {
        _context = context;
        AddParticipantDataToDb(participantService.Juniors, participantService.Teamleads);
    }

    public void AddMeanDataToDb(
        PreferencesStore<Junior, Teamlead> juniorsTeamleads,
        PreferencesStore<Teamlead, Junior> teamleadsJuniors,
        AssignmentStore<Teamlead, Junior> teams,
        Dictionary<Assignment<Teamlead, Junior>, int> scores,
        double harmonicMean
    ) 
    {
        var run = new HackatonRunScheme
        {
            HackatonId = HackatonId
        };
        _context.Add(run);
        _context.SaveChanges();
        _context.JuniorLists.AddRange(juniorsTeamleads.ToPreferencsScheme(HackatonId));
        _context.SaveChanges();
        _context.TeamleadLists.AddRange(teamleadsJuniors.ToPreferencsScheme(HackatonId));
        _context.SaveChanges();
        _context.Teams.AddRange(teams.ToTeamsScheme(run.Id));
        _context.SaveChanges();
        foreach (var (assignment, score) in scores)
        {
            var team = _context.Teams.Single(team => team.HackatonRunId == run.Id && team.TeamleadId == assignment.Teamlead.Id && team.JuniorId == assignment.Junior.Id);
            team.Score = score;
        }
        _context.SaveChanges();
        _context.HachatonRuns.Single(srun => srun.Id == run.Id).mean = harmonicMean;
        _context.SaveChanges();
    }

    private void AddParticipantDataToDb(List<Junior> Juniors, List<Teamlead> Teamleads)
    {
        _context.Database.EnsureCreated();

        var hackaton = new HackatonScheme();
        _context.Hachatons.Add(hackaton);
        _context.SaveChanges();
        HackatonId = hackaton.Id;

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

        // _context.JuniorLists.AddRange(juniorsTeamleads.ToPreferencsScheme(Id));
        // _context.TeamleadLists.AddRange(teamleadsJuniors.ToPreferencsScheme(Id));
        // _context.SaveChanges();
    }
}