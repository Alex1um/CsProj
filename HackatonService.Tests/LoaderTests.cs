namespace HackatonService.Tests;
using HackatonService.src.ObjectOriented;
using HackatonService.src.ObjectOriented.Participants;

public class LoaderTests
{
    internal static List<T> ReadFromPath<T>(string path) where T : Participant, new() {
        var full_path = PathUtils.GetFromProjectPath(path);
        var list = CSVReader.Read<T>(full_path);
        return list;
    }

    internal static List<T> ReadFromPath<T>(FullPath full_path) where T : Participant, new() {
        var list = CSVReader.Read<T>(full_path);
        return list;
    }

    void TestLineCount<T>(string path) where T : Participant, new() {
        var full_path = PathUtils.GetFromProjectPath(path);
        var list = ReadFromPath<T>(full_path);
        var lines = File.ReadLines(full_path).Count();
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