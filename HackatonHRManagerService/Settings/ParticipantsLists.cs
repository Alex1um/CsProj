using HackatonBase.Participants;
using HackatonBase.DataIO;


class ParticipantsLists {

    public List<Teamlead> Teamleads { get; set; } = CSVReader.Read<Teamlead>("Teamleads5.csv"); 
    public List<Junior> Juniors { get; set; } = CSVReader.Read<Junior>("Juniors5.csv");
}