namespace HackatonBase.DataIO;

using HackatonBase.Participants;

public static class CSVReader
{
    public static List<T> Read<T>(string file) where T : Participant, new()
    {
        var list = new List<T>();
        var readed = File.ReadAllText(file);

        foreach (var line in readed.Split("\n").Skip(1))
        {
            if (string.IsNullOrEmpty(line)) continue;
            var data = line.Split(';');
            var participant = new T
            {
                Id = int.Parse(data[0]),
                Name = data[1]
            };
            list.Add(participant);
        }
        return list;
    }
}