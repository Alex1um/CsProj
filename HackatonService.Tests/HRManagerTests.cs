namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using static HackatonService.Tests.SharedObjects;

public class HRManagerTests
{

    private void TestTeamCount(Dictionary<Teamlead, Junior> teams, List<Teamlead> teamleads, List<Junior> juniors) {
        Assert.Equal(teams.Count, int.Min(teamleads.Count, juniors.Count));
    }

    [Fact]
    public void TestRandomBuildLoadedTeam() {
        Assert.Equal(LoadedObjects.RandomTeams.Count, LoadedObjects.Teamleads.Count);
        Assert.Equal(LoadedObjects.RandomTeams.Count, LoadedObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildLoadedTeam() {
        Assert.Equal(LoadedObjects.OptimalTeams.Count, LoadedObjects.Teamleads.Count);
        Assert.Equal(LoadedObjects.OptimalTeams.Count, LoadedObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestRandomBuildCustomTeam() {
        Assert.Equal(CustomObjects.OptimalTeams.Count, CustomObjects.Teamleads.Count);
        Assert.Equal(CustomObjects.OptimalTeams.Count, CustomObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildCustomTeam() {
        Assert.Equal(CustomObjects.OptimalTeams.Count, CustomObjects.Teamleads.Count);
        Assert.Equal(CustomObjects.OptimalTeams.Count, CustomObjects.Juniors.Count);
    }

    [Fact]
    public void TestOptimalBuildingStrategyCorectness() {
        Assert.Equal(CustomObjects.BuildedTeams, CustomObjects.OptimalTeams);
    }

}
