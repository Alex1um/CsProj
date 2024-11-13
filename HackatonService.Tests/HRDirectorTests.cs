namespace HackatonService.Tests;
using HackatonService.Extensions;
using HackatonService;
using HackatonService.Participants;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;
using Moq;

public class HRDirectorTests
{


    [Fact]
    public void TestMeanHarmonicCorectness()
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
        AssignmentStore<Teamlead, Junior> buildedTeams = new(new()
        {
            [teamlead1] = jun1,
            [teamlead2] = jun2
        });
        var dbMock = new Mock<HackatonDbContext>();
        var context = dbMock.Object;

        HRDirector HRDirectorInstance = new(context);

        Dictionary<Assignment<Teamlead, Junior>, int> scores = HRDirectorInstance.CalcSatisfactionIndex(0, teamleadsJuniors, juniorsTeamleads, buildedTeams);
        
        double mean = scores.Values.ToList().GetHarmonicMean();
        foreach (var (key, value) in scores)
        {
            Assert.Equal(value, mean);
        }
    }

    [Theory]
    [InlineData(new int[] { 2, 6 }, 3)]
    [InlineData(new int[] { 3, 2, 1, 6 }, 2)]
    public void TestMeanHarmonicValues(int[] values, double expected)
    {
        var evaled = values.ToList().GetHarmonicMean();
        Assert.Equal(expected, evaled);
    }
}