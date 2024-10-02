namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;
using static HackatonService.Tests.SharedObjects;
using static HackatonService.Tests.SharedObjects.CustomObjects;
using ParticipantAssignment = (HackatonService.src.ObjectOriented.Participants.Teamlead, HackatonService.src.ObjectOriented.Participants.Junior);


public class HRDirectorTests
{

    internal static Dictionary<ParticipantAssignment, int> scores = hRDirector.CalcSatisfactionIndex(teamleadsJuniors, juniorsTeamleads, buildedTeams);
    internal static double mean = hRDirector.GetHarmonicMean(scores);

    [Fact]
    public void TestMeanHarmonicCorectness()
    {
        foreach (var (key, value) in scores)
        {
            Assert.Equal(value, mean);
        }
    }
    
    [Theory]
    [InlineData(new int[] {2, 6}, 3)]
    [InlineData(new int[] {3, 2, 1, 6}, 2)]
    public void TestMeanHarmonicValues(int[] values, double expected)
    {
        var evaled = hRDirector.GetHarmonicMean(values.ToList());
        Assert.Equal(expected, evaled);
    }
}