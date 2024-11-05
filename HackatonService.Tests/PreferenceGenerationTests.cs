namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;

public class PreferenceGenerationTests {


    private void TestPreferListLengths<T, U>(PreferencesStore<T, U> preferList, List<T> units, List<U> preferable) where T : notnull where U : Participant{
        Assert.Equal(units.Count, preferList.Count);
        foreach (var (unit, prefered) in preferList)
        {
            Assert.Equal(prefered.Count, preferable.Count);
        }
    }
    
    private void TestPreferListRepeatations<T, U>(PreferencesStore<T, U> preferList) where T : notnull where U : Participant{
        foreach (var (unit, prefered) in preferList)
        {
            var hs = new HashSet<U>(prefered);
            Assert.Equal(prefered.Count, hs.Count);
        }
    }
    
    private void TestAllContainsName<T, U>(PreferencesStore<T, U> preferList, string participant) where T : notnull where U : Participant{
        foreach (var (unit, prefered) in preferList)
        {
            Assert.Contains(participant, prefered.Select(p => p.Name));
        }
    }

    [Fact]
    public void TestJuniorsTeamleads() {
        var juniors = new List<Junior>() {
            new() { Name = "Junior1"},
            new() { Name = "Junior2"},
            new() { Name = "Junior3"},
            new() { Name = "Junior4"},
        };
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1"},
            new() { Name = "Teamlead2"},
            new() { Name = "Teamlead3"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Junior, Teamlead> preferList = new(juniors, teamleads);
        TestPreferListLengths(preferList, juniors, teamleads);
        TestPreferListRepeatations(preferList);
    }
    
    [Fact]
    public void TestTeamleadsJuniors() {
        var juniors = new List<Junior>() {
            new() { Name = "Junior3"},
            new() { Name = "Junior4"},
        };
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead3"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Teamlead, Junior> preferList = new(teamleads, juniors);
        TestPreferListLengths(preferList, teamleads, juniors);
        TestPreferListRepeatations(preferList);
    }

    [Theory]
    [InlineData("Филиппова Ульяна")]
    [InlineData("Климов Михаил")]
    public void ContainsTeamleadEntry(string name) {
        var juniors = new List<Junior>() {
            new() { Name = "Junior1"},
            new() { Name = "Junior2"},
            new() { Name = "Junior3"},
            new() { Name = "Junior4"},
        };
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1"},
            new() { Name = "Климов Михаил"},
            new() { Name = "Филиппова Ульяна"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Junior, Teamlead> preferList = new(juniors, teamleads);
        TestAllContainsName(preferList, name);
    }
    
    [Theory]
    [InlineData("Яшина Яна")]
    [InlineData("Кузьмин Глеб")]
    public void ContainsJuniorEntry(string name) {
        var juniors = new List<Junior>() {
            new() { Name = "Junior1"},
            new() { Name = "Яшина Яна"},
            new() { Name = "Кузьмин Глеб"},
            new() { Name = "Junior4"},
        };
        var teamleads = new List<Teamlead>() {
            new() { Name = "Teamlead1"},
            new() { Name = "Teamlead2"},
            new() { Name = "Teamlead3"},
            new() { Name = "Teamlead4"},
        };
        PreferencesStore<Teamlead, Junior> preferList = new(teamleads, juniors);
        TestAllContainsName(preferList, name);
    }
}