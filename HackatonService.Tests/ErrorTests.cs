namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;

public class ErrorTests
{
    [Fact]
    public void WrongPreferenceStoreTest()
    {
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1" },
            new() { Name = "Teamlead2" },
            new() { Name = "Teamlead3" },
            new() { Name = "Teamlead4" },
        };
        var juniors = new List<Junior>() {
            new() { Name = "Junior1" },
            new() { Name = "Junior2" },
        };
        Assert.NotEqual(teamleads.Count, juniors.Count);
        Assert.Throws<Exception>(() => new PreferencesStore<Teamlead, Junior>(teamleads, juniors));
    }

    [Fact]
    public void AssignmentStoreRandomGenerationTest()
    {
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1" },
            new() { Name = "Teamlead2" },
            new() { Name = "Teamlead3" },
            new() { Name = "Teamlead4" },
        };
        var teamleads5 = new List<Teamlead>() {
            new() { Name = "Teamlead1" },
            new() { Name = "Teamlead2" },
        };
        var juniors = new List<Junior>() {
            new() { Name = "Junior1" },
            new() { Name = "Junior2" },
            new() { Name = "Junior3" },
            new() { Name = "Junior4" },
        };
        var juniors5 = new List<Junior>() {
            new() { Name = "Junior1" },
            new() { Name = "Junior2" },
        };
        PreferencesStore<Junior, Teamlead> juniorsTeamleads = new(juniors, teamleads);
        PreferencesStore<Teamlead, Junior> teamleadsJuniors = new(teamleads, juniors);

        ITeamBuildingStrategy random_strategy = new RandomTeamBuildingStrategy();
        Assert.Throws<Exception>(() => random_strategy.BuildTeams(teamleads5, juniors, teamleadsJuniors, juniorsTeamleads));
    }

    [Fact]
    public void AssignmentStoreStableGenerationTest()
    {
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1" },
            new() { Name = "Teamlead2" },
            new() { Name = "Teamlead3" },
            new() { Name = "Teamlead4" },
        };
        var juniors = new List<Junior>() {
            new() { Name = "Junior1" },
            new() { Name = "Junior2" },
            new() { Name = "Junior3" },
            new() { Name = "Junior4" },
        };
        var teamleads5 = new List<Teamlead>() {
            new() { Name = "Teamlead1" },
            new() { Name = "Teamlead2" },
        };
        var juniors5 = new List<Junior>() {
            new() { Name = "Junior1" },
            new() { Name = "Junior2" },
        };
        ITeamBuildingStrategy optimalStrategy = new StableMarriageTeamBuildingStrategy();
        
        PreferencesStore<Junior, Teamlead> juniorsTeamleads = new(juniors, teamleads);
        PreferencesStore<Teamlead, Junior> teamleadsJuniors = new(teamleads, juniors);
        PreferencesStore<Junior, Teamlead> juniorsTeamleads5 = new(juniors5, teamleads5);
        PreferencesStore<Teamlead, Junior> teamleadsJuniors5 = new(teamleads5, juniors5);

        Assert.Throws<Exception>(() => optimalStrategy.BuildTeams(teamleads, juniors5, teamleadsJuniors, juniorsTeamleads5));
        Assert.Throws<Exception>(() => optimalStrategy.BuildTeams(teamleads, juniors, teamleadsJuniors, juniorsTeamleads5));
        Assert.Throws<Exception>(() => optimalStrategy.BuildTeams(teamleads, juniors5, teamleadsJuniors, juniorsTeamleads));
    }
}