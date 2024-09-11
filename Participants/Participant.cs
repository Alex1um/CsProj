abstract class Participant()
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";

    public Participant(int id, string name) : this() {
        Id = id;
        Name = name;
    }

    public JuniorList createList<T>(List<T> other_list) where T: Participant {
        var list = new JuniorList(Id);
        // fill juniorList with random teamlead ids
        var random = new Random();
        var teamlead_ids = other_list.Select(x => x.Id).ToArray();
        random.Shuffle(teamlead_ids);
        for (int i = 0; i < list.Members.Length; i++) list.Members[i] = teamlead_ids[i];
        return list;
    }
}