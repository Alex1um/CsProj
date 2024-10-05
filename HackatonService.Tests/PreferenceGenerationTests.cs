namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using static HackatonService.Tests.SharedObjects.LoadedObjects;

public class PreferenceGenerationTests {


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
        var preferList = JuniorsTeamleads;
        TestPreferListLengths(preferList, Juniors, Teamleads);
        TestPreferListRepeatations(preferList);
    }
    
    [Fact]
    public void TestTeamleadsJuniors() {
        var preferList = TeamleadsJuniors;
        TestPreferListLengths(preferList, Teamleads, Juniors);
        TestPreferListRepeatations(preferList);
    }

    [Theory]
    [InlineData("Филиппова Ульяна")]
    [InlineData("Климов Михаил")]
    public void ContainsTeamleadEntry(string name) {
        var preferList = JuniorsTeamleads;
        TestAllContainsName(preferList, name);
    }
    
    [Theory]
    [InlineData("Яшина Яна")]
    [InlineData("Кузьмин Глеб")]
    public void ContainsJuniorEntry(string name) {
        var preferList = TeamleadsJuniors;
        TestAllContainsName(preferList, name);
    }
}