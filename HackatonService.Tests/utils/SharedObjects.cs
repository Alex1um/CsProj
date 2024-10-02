namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;

internal static class SharedObjects {
    internal static readonly ITeamBuildingStrategy randomStrategy = new RandomTeamBuildingStrategy();
    internal static readonly ITeamBuildingStrategy optimalStrategy = new StableMarriageTeamBuildingStrategy();
    internal static readonly HRManager randomManager = new(randomStrategy);
    internal static readonly HRManager optimalManager = new(optimalStrategy);
    internal static readonly HRDirector hRDirector = new();
    
    internal static class CustomObjects {

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

        internal static readonly PreferList<Junior, Teamlead> juniorsTeamleads = new(new Dictionary<Junior, List<Teamlead>>() {
            [jun1] = [teamlead1, teamlead2],
            [jun2] = [teamlead2, teamlead1]
        });
        
        internal static readonly PreferList<Teamlead, Junior> teamleadsJuniors = new(new Dictionary<Teamlead, List<Junior>>() {
            [teamlead1] = [jun1, jun2],
            [teamlead2] = [jun2, jun1]
        });

        internal static readonly Dictionary<Teamlead, Junior> buildedTeams = new() {
            [teamlead1] = jun1,
            [teamlead2] = jun2
        };

        internal static readonly Dictionary<Teamlead, Junior> randomTeams = randomManager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads); 
        internal static readonly Dictionary<Teamlead, Junior> optimalTeams = optimalManager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads); 
    } 

    internal static class LoadedObjects {
        internal static readonly List<Teamlead> Teamleads = LoaderTests.ReadFromPath<Teamlead>("CSHARP_2024_NSU/Teamleads20.csv");
        internal static readonly List<Junior> Juniors = LoaderTests.ReadFromPath<Junior>("CSHARP_2024_NSU/Juniors20.csv");
        internal static readonly PreferList<Junior, Teamlead> juniorsTeamleads = new(Juniors, Teamleads);
        internal static readonly PreferList<Teamlead, Junior> teamleadsJuniors = new(Teamleads, Juniors);    
        internal static readonly Dictionary<Teamlead, Junior> randomTeams = randomManager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads); 
        internal static readonly Dictionary<Teamlead, Junior> optimalTeams = optimalManager.BuildTeams(Teamleads, Juniors, teamleadsJuniors, juniorsTeamleads); 
    }
}