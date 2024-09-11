static class CSVReader {
    public static List<T> Read<T>(string file) where T : Participant, new() {
        var list = new List<T>();
        var readed = File.ReadAllText(file);

        foreach (var line in readed.Split("\n").Skip(1)) {
            if (string.IsNullOrEmpty(line)) continue;
            var participant = new T();
            var data = line.Split(';');
            participant.Id = int.Parse(data[0]);
            participant.Name = data[1];
            list.Add(participant);
        }
        return list;
    }
}