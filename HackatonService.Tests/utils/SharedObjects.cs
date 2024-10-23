namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;

internal static class SharedObjects
{
    internal static readonly ITeamBuildingStrategy RandomStrategy = new RandomTeamBuildingStrategy();
    internal static readonly ITeamBuildingStrategy OptimalStrategy = new StableMarriageTeamBuildingStrategy();
    internal static readonly HRManager RandomManager = new(RandomStrategy);
    internal static readonly HRManager OptimalManager = new(OptimalStrategy);
    internal static readonly HRDirector HRDirectorInstance = new();

    internal static class CustomObjects
    {

        static readonly Junior jun1 = new()
        {
            Id = 0,
            Name = "jun1"
        };

        static readonly Junior jun2 = new()
        {
            Id = 0,
            Name = "jun2"
        };

        static readonly Teamlead teamlead1 = new()
        {
            Id = 0,
            Name = "teamlead1"
        };

        static readonly Teamlead teamlead2 = new()
        {
            Id = 0,
            Name = "teamlead2"
        };

        internal static readonly List<Junior> Juniors = [jun1, jun2];
        internal static readonly List<Teamlead> Teamleads = [teamlead1, teamlead2];

        internal static readonly PreferencesStore<Junior, Teamlead> JuniorsTeamleads = new(new Dictionary<Junior, List<Teamlead>>()
        {
            [jun1] = [teamlead1, teamlead2],
            [jun2] = [teamlead2, teamlead1]
        });

        internal static readonly PreferencesStore<Teamlead, Junior> TeamleadsJuniors = new(new Dictionary<Teamlead, List<Junior>>()
        {
            [teamlead1] = [jun1, jun2],
            [teamlead2] = [jun2, jun1]
        });

        internal static readonly AssignmentStore<Teamlead, Junior> BuildedTeams = new(new()
        {
            [teamlead1] = jun1,
            [teamlead2] = jun2
        });

        internal static readonly AssignmentStore<Teamlead, Junior> RandomTeams = RandomManager.BuildTeams(Teamleads, Juniors, TeamleadsJuniors, JuniorsTeamleads);
        internal static readonly AssignmentStore<Teamlead, Junior> OptimalTeams = OptimalManager.BuildTeams(Teamleads, Juniors, TeamleadsJuniors, JuniorsTeamleads);
    }

    internal static class LoadedObjects
    {
        internal static readonly List<Teamlead> Teamleads = LoaderTests.ReadFromPath<Teamlead>("CSHARP_2024_NSU/Teamleads20.csv");
        internal static readonly List<Junior> Juniors = LoaderTests.ReadFromPath<Junior>("CSHARP_2024_NSU/Juniors20.csv");
        internal static readonly PreferencesStore<Junior, Teamlead> JuniorsTeamleads = new(Juniors, Teamleads);
        internal static readonly PreferencesStore<Teamlead, Junior> TeamleadsJuniors = new(Teamleads, Juniors);
        internal static readonly AssignmentStore<Teamlead, Junior> RandomTeams = RandomManager.BuildTeams(Teamleads, Juniors, TeamleadsJuniors, JuniorsTeamleads);
        internal static readonly AssignmentStore<Teamlead, Junior> OptimalTeams = OptimalManager.BuildTeams(Teamleads, Juniors, TeamleadsJuniors, JuniorsTeamleads);
    }
    internal static class LoadedObjects5
    {
        internal static readonly List<Teamlead> Teamleads5 = LoaderTests.ReadFromPath<Teamlead>("CSHARP_2024_NSU/Teamleads5.csv");
        internal static readonly List<Junior> Juniors5 = LoaderTests.ReadFromPath<Junior>("CSHARP_2024_NSU/Juniors5.csv");
        internal static readonly PreferencesStore<Junior, Teamlead> JuniorsTeamleads5 = new(Juniors5, Teamleads5);
        internal static readonly PreferencesStore<Teamlead, Junior> TeamleadsJuniors5 = new(Teamleads5, Juniors5);
        internal static readonly AssignmentStore<Teamlead, Junior> RandomTeams5 = RandomManager.BuildTeams(Teamleads5, Juniors5, TeamleadsJuniors5, JuniorsTeamleads5);
        internal static readonly AssignmentStore<Teamlead, Junior> OptimalTeams5 = OptimalManager.BuildTeams(Teamleads5, Juniors5, TeamleadsJuniors5, JuniorsTeamleads5);
    }
}