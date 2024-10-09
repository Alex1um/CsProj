namespace HackatonService.Tests;
using HackatonService;
using HackatonService.Participants;
using HackatonService.DataIO;

public class LoaderTests
{
    internal static List<T> ReadFromPath<T>(string path) where T : Participant, new() {
        var fullPath = PathUtils.GetFromProjectPath(path);
        var list = CSVReader.Read<T>(fullPath);
        return list;
    }

    internal static List<T> ReadFromPath<T>(FullPath fullPath) where T : Participant, new() {
        var list = CSVReader.Read<T>(fullPath);
        return list;
    }

    void TestLineCount<T>(string path) where T : Participant, new() {
        var fullPath = PathUtils.GetFromProjectPath(path);
        var list = ReadFromPath<T>(fullPath);
        var lines = File.ReadLines(fullPath).Count();
        Assert.Equal(lines - 1, list.Count);
    }

    [Theory]
    [InlineData("CSHARP_2024_NSU/Teamleads20.csv")]
    [InlineData("CSHARP_2024_NSU/Teamleads5.csv")]
    public void TestLoadTeamleads(string path)
    {
        TestLineCount<Teamlead>(path);
    }

    [Theory]
    [InlineData("CSHARP_2024_NSU/Juniors20.csv")]
    [InlineData("CSHARP_2024_NSU/Juniors5.csv")]
    public void TestLoadJuniors(string path)
    {
        TestLineCount<Junior>(path);
    }
}