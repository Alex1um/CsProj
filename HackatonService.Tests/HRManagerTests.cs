namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;
using Moq;

public class HRManagerTests
{

    private void TestTeamCount(Dictionary<Teamlead, Junior> teams, List<Teamlead> teamleads, List<Junior> juniors) {
        Assert.Equal(teams.Count, int.Min(teamleads.Count, juniors.Count));
    }

    [Fact]
    public void TestRandomBuildLoadedTeam() {
        
        List<Junior> juniors = new() {
            new() { Name = "Junior1"},
            new() { Name = "Junior2"},
            new() { Name = "Junior3"},
            new() { Name = "Junior4"},
        };
        List<Teamlead> teamleads = new() {
            new() { Name = "Teamlead1"},
            new() { Name = "Teamlead2"},
            new() { Name = "Teamlead3"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Junior, Teamlead> juniorsTeamleads = new(juniors, teamleads);
        PreferencesStore<Teamlead, Junior> teamleadsJuniors = new(teamleads, juniors);

        ITeamBuildingStrategy randomStrategy = new RandomTeamBuildingStrategy();
        
        var dbMock = new Mock<HackatonDbContext>();
        var context = dbMock.Object;
        HRManager randomManager = new(randomStrategy, context);

        AssignmentStore<Teamlead, Junior> randomTeams = randomManager.BuildTeams(0, teamleads, juniors, teamleadsJuniors, juniorsTeamleads);
        Assert.Equal(randomTeams.Count, teamleads.Count);
        Assert.Equal(randomTeams.Count, juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildLoadedTeam() {
        List<Junior> juniors = new() {
            new() { Name = "Junior1"},
            new() { Name = "Junior2"},
            new() { Name = "Junior3"},
            new() { Name = "Junior4"},
        };
        List<Teamlead> teamleads = new() {
            new() { Name = "Teamlead1"},
            new() { Name = "Teamlead2"},
            new() { Name = "Teamlead3"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Junior, Teamlead> juniorsTeamleads = new(juniors, teamleads);
        PreferencesStore<Teamlead, Junior> teamleadsJuniors = new(teamleads, juniors);

        ITeamBuildingStrategy optimalStrategy = new StableMarriageTeamBuildingStrategy();
        var dbMock = new Mock<HackatonDbContext>();
        var context = dbMock.Object;
        HRManager optimalManager = new(optimalStrategy, context);

        AssignmentStore<Teamlead, Junior> optimalTeams = optimalManager.BuildTeams(0, teamleads, juniors, teamleadsJuniors, juniorsTeamleads);
        Assert.Equal(optimalTeams.Count, teamleads.Count);
        Assert.Equal(optimalTeams.Count, juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildingStrategyCorectness() {
        Junior jun1 = new()
        {
            Id = 0,
            Name = "jun1"
        };

        Junior jun2 = new()
        {
            Id = 0,
            Name = "jun2"
        };

        Teamlead teamlead1 = new()
        {
            Id = 0,
            Name = "teamlead1"
        };

        Teamlead teamlead2 = new()
        {
            Id = 0,
            Name = "teamlead2"
        };
        AssignmentStore<Teamlead, Junior> buildedTeams = new(new()
        {
            [teamlead1] = jun1,
            [teamlead2] = jun2
        });
        
        List<Junior> juniors = [jun1, jun2];
        List<Teamlead> teamleads = [teamlead1, teamlead2];
        
        PreferencesStore<Junior, Teamlead> juniorsTeamleads = new(new Dictionary<Junior, List<Teamlead>>()
        {
            [jun1] = [teamlead1, teamlead2],
            [jun2] = [teamlead2, teamlead1]
        });

        PreferencesStore<Teamlead, Junior> teamleadsJuniors = new(new Dictionary<Teamlead, List<Junior>>()
        {
            [teamlead1] = [jun1, jun2],
            [teamlead2] = [jun2, jun1]
        });
        ITeamBuildingStrategy optimalStrategy = new StableMarriageTeamBuildingStrategy();
        var dbMock = new Mock<HackatonDbContext>();
        var context = dbMock.Object;
        HRManager optimalManager = new(optimalStrategy, context);
        
        AssignmentStore<Teamlead, Junior> optimalTeams = optimalManager.BuildTeams(0, teamleads, juniors, teamleadsJuniors, juniorsTeamleads);

        Assert.Equal(buildedTeams, optimalTeams);
    }

}
