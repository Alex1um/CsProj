class Junior() : Participant() {

    public JuniorList createList(List<Teamlead> teamleads) {
        var list = new JuniorList(Id);
        // fill juniorList with random teamlead ids
        var random = new Random();
        var teamlead_ids = teamleads.Select(x => x.Id).ToArray();
        random.Shuffle(teamlead_ids);
        for (int i = 0; i < list.Members.Count(); i++) list.Members[i] = teamlead_ids[i];
        return list;
    }

}