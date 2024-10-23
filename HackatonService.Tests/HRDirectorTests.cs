namespace HackatonService.Tests;
using static HackatonService.Tests.SharedObjects;
using static HackatonService.Tests.SharedObjects.CustomObjects;
using HackatonService.Extensions;
using HackatonService;
using HackatonService.Participants;

public class HRDirectorTests
{

    internal static Dictionary<Assignment<Teamlead, Junior>, int> scores = HRDirectorInstance.CalcSatisfactionIndex(TeamleadsJuniors, JuniorsTeamleads, BuildedTeams);
    internal static double mean = scores.Values.ToList().GetHarmonicMean();

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
        var evaled = values.ToList().GetHarmonicMean();
        Assert.Equal(expected, evaled);
    }
}