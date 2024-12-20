namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;

public class HackatonTests(DatabaseSqliteFixture fixture) : IClassFixture<DatabaseSqliteFixture>
{
    readonly DatabaseSqliteFixture fixture = fixture;

    [Fact]
    public void TestHackatonRun()
    {
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
        var context = fixture.context;

        var hackaton = new Hackaton([teamlead1, teamlead2], [jun1, jun2], juniorsTeamleads, teamleadsJuniors, context);

        var result = hackaton.Run(new HRManager(new StableMarriageTeamBuildingStrategy()), new HRDirector());

        Assert.Equal(4, result);
    }
}