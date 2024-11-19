namespace HackatonService.TUI;

using HackatonService.DB;
using Microsoft.VisualBasic;
using Spectre.Console;

class TerminalUserInterface(
    HackatonDbContext context,
    Hackaton hackaton,
    HRManager manager,
    HRDirector director
)
{
    private readonly HackatonDbContext _context = context;
    private readonly Hackaton _hackaton = hackaton;
    private readonly HRManager _manager = manager;
    private readonly HRDirector _director = director;


    private void RunHackaton()
    {
        var harmonicMean = _hackaton.Run(_manager, _director);
        AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(["Ok"]).Title($"Mean Harmonic: {harmonicMean}"));
    }

    private void GetHackatonParticipants()
    {
        var hackatonIds = _context.HachatonRuns.Select(run => run.HackatonId).ToList();
        var selectedId = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Select Hackaton")
                .AddChoices(hackatonIds)
        );
        var junior_ids = _context.JuniorLists.Where(list => list.HackatonId == selectedId).Select(list => list.UnitId).ToList();
        var teamlead_ids = _context.TeamleadLists.Where(list => list.HackatonId == selectedId).Select(list => list.UnitId).ToList();
        var junior_names = _context.Juniors.Where(junior => junior_ids.Contains(junior.Id)).Select(junior => junior.Name).ToList();
        var teamlead_names = _context.Teamleads.Where(teamlead => teamlead_ids.Contains(teamlead.Id)).Select(teamlead => teamlead.Name).ToList();
        var participants = junior_names.Union(teamlead_names);
        var table = new Table();
        table.AddColumn("Participants");
        foreach (var participant in participants)
        {
            table.AddRow(participant);
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("\n");
        AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(["Ok"]));
    }

    private void GetMeanHarmonic() {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Mean");
        _context.HachatonRuns
            .GroupBy(run => run.HackatonId)
            .ToList()
            .ForEach(group => table.AddRow(new Text(group.Key.ToString()), new Text(group.Average(run => run.mean).ToString())));
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("\n");
        AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(["Ok"]));
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Reset();
            AnsiConsole.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Menu")
                .PageSize(10)
                .AddChoices([
                    "Run Hackaton",
                    "Get Hackaton Participants",
                    "Get Mean Harmonic",
                    "Exit"
                ])
            );

            switch (option)
            {
                case "Run Hackaton":
                    RunHackaton();
                    break;
                case "Get Hackaton Participants":
                    GetHackatonParticipants();
                    break;
                case "Get Mean Harmonic":
                    GetMeanHarmonic();
                    break;
                case "Exit":
                    return;
            }
        }
    }
}