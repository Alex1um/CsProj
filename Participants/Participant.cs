abstract class Participant()
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";

    public Participant(int id, string name) : this() {
        Id = id;
        Name = name;
    }

    public List<T> createList<T>(List<T> other_list) where T : Participant
    {
        // var arr = other_list.ToArray();
        var arr = other_list.ToList();
        var random = new Random();
        for (int i = 0; i < arr.Count; i++)
        {
            var j = random.Next(i, arr.Count);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        return [.. arr];
    }
}