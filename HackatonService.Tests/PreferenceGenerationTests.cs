namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;

public class PreferenceGenerationTests {

    private readonly List<Teamlead> Teamleads = LoaderTests.ReadFromPath<Teamlead>("CSHARP_2024_NSU/Teamleads20.csv");
    private readonly List<Junior> Juniors = LoaderTests.ReadFromPath<Junior>("CSHARP_2024_NSU/Juniors20.csv");

    private void TestPreferListLengths<T, U>(PreferList<T, U> preferList, List<T> units, List<U> preferable) where T : notnull where U : Participant{
        Assert.Equal(units.Count, preferList.Count);
        foreach (var (unit, prefered) in preferList)
        {
            Assert.Equal(prefered.Count, preferable.Count);
        }
    }
    
    private void TestPreferListRepeatations<T, U>(PreferList<T, U> preferList) where T : notnull where U : Participant{
        foreach (var (unit, prefered) in preferList)
        {
            var hs = new HashSet<U>(prefered);
            Assert.Equal(prefered.Count, hs.Count);
        }
    }

    [Fact]
    public void TestJuniorsTeamleads() {
        var preferList = new PreferList<Junior, Teamlead>(Juniors, Teamleads);
        TestPreferListLengths(preferList, Juniors, Teamleads);
        TestPreferListRepeatations(preferList);
    }
    
    [Fact]
    public void TestTeamleadsJuniors() {
        var preferList = new PreferList<Teamlead, Junior>(Teamleads, Juniors);
        TestPreferListLengths(preferList, Teamleads, Juniors);
        TestPreferListRepeatations(preferList);
    }
}