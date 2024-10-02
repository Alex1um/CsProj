namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;

public class PreferenceGenerationTests {

    private static readonly List<Teamlead> Teamleads = LoaderTests.ReadFromPath<Teamlead>("CSHARP_2024_NSU/Teamleads20.csv");
    private static readonly List<Junior> Juniors = LoaderTests.ReadFromPath<Junior>("CSHARP_2024_NSU/Juniors20.csv");
    private static readonly PreferList<Junior, Teamlead> juniorsTeamleads = new PreferList<Junior, Teamlead>(Juniors, Teamleads);
    private static readonly PreferList<Teamlead, Junior> teamleadsJuniors = new PreferList<Teamlead, Junior>(Teamleads, Juniors);    

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
    
    private void TestAllContainsName<T, U>(PreferList<T, U> preferList, string participant) where T : notnull where U : Participant{
        foreach (var (unit, prefered) in preferList)
        {
            Assert.Contains(participant, prefered.Select(p => p.Name));
        }
    }

    [Fact]
    public void TestJuniorsTeamleads() {
        var preferList = juniorsTeamleads;
        TestPreferListLengths(preferList, Juniors, Teamleads);
        TestPreferListRepeatations(preferList);
    }
    
    [Fact]
    public void TestTeamleadsJuniors() {
        var preferList = teamleadsJuniors;
        TestPreferListLengths(preferList, Teamleads, Juniors);
        TestPreferListRepeatations(preferList);
    }

    [Theory]
    [InlineData("Филиппова Ульяна")]
    [InlineData("Климов Михаил")]
    public void ContainsTeamleadEntry(string name) {
        var preferList = juniorsTeamleads;
        TestAllContainsName(preferList, name);
    }
    
    [Theory]
    [InlineData("Яшина Яна")]
    [InlineData("Кузьмин Глеб")]
    public void ContainsJuniorEntry(string name) {
        var preferList = teamleadsJuniors;
        TestAllContainsName(preferList, name);
    }
}