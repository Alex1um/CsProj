namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using static HackatonService.Tests.SharedObjects;
using static HackatonService.Tests.SharedObjects.CustomObjects;
using ParticipantAssignment = (HackatonService.Participants.Teamlead, HackatonService.Participants.Junior);


public class HRDirectorTests
{

    internal static Dictionary<ParticipantAssignment, int> scores = HRDirectorInstance.CalcSatisfactionIndex(TeamleadsJuniors, JuniorsTeamleads, BuildedTeams);
    internal static double mean = HRDirectorInstance.GetHarmonicMean(scores);

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
        var evaled = HRDirectorInstance.GetHarmonicMean(values.ToList());
        Assert.Equal(expected, evaled);
    }
}