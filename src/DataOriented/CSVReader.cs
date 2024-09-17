namespace CsProj.src.DataOriented;

static class CSVReader
{
    public static T Read<T>(string file) where T: List<Participant>, new()
    {
        var list = new T();
        var readed = File.ReadAllText(file);

        foreach (var line in readed.Split("\n").Skip(1))
        {
            if (string.IsNullOrEmpty(line)) continue;
            var data = line.Split(';');
            var participant = new Participant(int.Parse(data[0]), data[1]);
            list.Add(participant);
        }
        return list;
    }
}