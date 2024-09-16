abstract class Participant()
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";

    public Participant(int id, string name) : this() {
        Id = id;
        Name = name;
    }

    public List<T> createList<T>(List<T> other_list) where T: Participant {
        var arr = new T[20];
        // fill juniorList with random teamlead ids
        var random = new Random();
        for (int i = 0; i < arr.Count(); i++) arr[i] = other_list[i];
        random.Shuffle(arr);
        return [.. arr];
    }
}