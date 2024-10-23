namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using HackatonService.DataIO;

public class ErrorTests
{
    [Fact]
    public void WrongPreferenceStoreTest()
    {
        Assert.NotEqual(SharedObjects.LoadedObjects.Teamleads.Count, SharedObjects.LoadedObjects5.Juniors5.Count);
        Assert.Throws<Exception>(() => new PreferencesStore<Teamlead, Junior>(SharedObjects.LoadedObjects.Teamleads, SharedObjects.LoadedObjects5.Juniors5));
    }

    [Fact]
    public void AssignmentStoreRandomGenerationTest()
    {
        Assert.Throws<Exception>(() => SharedObjects.RandomStrategy.BuildTeams(SharedObjects.LoadedObjects.Teamleads, SharedObjects.LoadedObjects5.Juniors5, SharedObjects.LoadedObjects.TeamleadsJuniors, SharedObjects.LoadedObjects.JuniorsTeamleads));
    }
    
    [Fact]
    public void AssignmentStoreStableGenerationTest()
    {
        Assert.Throws<Exception>(() => SharedObjects.OptimalStrategy.BuildTeams(SharedObjects.LoadedObjects.Teamleads, SharedObjects.LoadedObjects5.Juniors5, SharedObjects.LoadedObjects.TeamleadsJuniors, SharedObjects.LoadedObjects5.JuniorsTeamleads5));
        Assert.Throws<Exception>(() => SharedObjects.OptimalStrategy.BuildTeams(SharedObjects.LoadedObjects.Teamleads, SharedObjects.LoadedObjects.Juniors, SharedObjects.LoadedObjects.TeamleadsJuniors, SharedObjects.LoadedObjects5.JuniorsTeamleads5));
        Assert.Throws<Exception>(() => SharedObjects.OptimalStrategy.BuildTeams(SharedObjects.LoadedObjects.Teamleads, SharedObjects.LoadedObjects5.Juniors5, SharedObjects.LoadedObjects.TeamleadsJuniors, SharedObjects.LoadedObjects.JuniorsTeamleads));
    }
}