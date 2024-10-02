namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;
using static HackatonService.Tests.SharedObjects;

public class HRManagerTests
{

    private void TestTeamCount(Dictionary<Teamlead, Junior> teams, List<Teamlead> teamleads, List<Junior> juniors) {
        Assert.Equal(teams.Count, int.Min(teamleads.Count, juniors.Count));
    }

    [Fact]
    public void TestRandomBuildLoadedTeam() {
        Assert.Equal(LoadedObjects.randomTeams.Count, LoadedObjects.Teamleads.Count);
        Assert.Equal(LoadedObjects.randomTeams.Count, LoadedObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildLoadedTeam() {
        Assert.Equal(LoadedObjects.optimalTeams.Count, LoadedObjects.Teamleads.Count);
        Assert.Equal(LoadedObjects.optimalTeams.Count, LoadedObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestRandomBuildCustomTeam() {
        Assert.Equal(CustomObjects.optimalTeams.Count, CustomObjects.Teamleads.Count);
        Assert.Equal(CustomObjects.optimalTeams.Count, CustomObjects.Juniors.Count);
    }
    
    [Fact]
    public void TestOptimalBuildCustomTeam() {
        Assert.Equal(CustomObjects.optimalTeams.Count, CustomObjects.Teamleads.Count);
        Assert.Equal(CustomObjects.optimalTeams.Count, CustomObjects.Juniors.Count);
    }

    [Fact]
    public void TestOptimalBuildingStrategyCorectness() {
        Assert.Equal(CustomObjects.buildedTeams, CustomObjects.optimalTeams);
    }

}
