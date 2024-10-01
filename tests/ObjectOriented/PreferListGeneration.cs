namespace CsProj.tests.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;
using CsProj.src.ObjectOriented;
using NUnit.Framework;


[TestFixture]
public class PreferListGeneration
{

    List<Teamlead> teamleads;
    List<Junior> juniors;

    String teamleadPath = "CSHARP_2024_NSU/Teamleads20.csv";
    String juniorsPath = "CSHARP_2024_NSU/Juniors20.csv";

    [SetUp]
    public void Setup() {
        teamleads = CSVReader.Read<Teamlead>(teamleadPath);
        juniors = CSVReader.Read<Junior>(juniorsPath);
    }

    [Test]
    public void JuniorsPreferListGeneration()
    {
        PreferList<Junior, Teamlead> junLists = new(juniors, teamleads);
        Assert.Equals(junLists.Count, juniors.Count);
    }
    [Test]
    public void TeamleadsPreferListGeneration()
    {
        PreferList<Teamlead, Junior> teamleadLists = new(teamleads, juniors);
        Assert.Equals(teamleadLists.Count, teamleads.Count);
    }
    [Test]
    public void FalseTest()
    {
        Assert.Equals(1, 2);
    }
}