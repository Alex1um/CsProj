namespace CsProj.HackatonApp.DataOriented;

readonly struct Participant(int Id, String name)
{
    public readonly int Id = Id;
    public readonly String name = name;
}

class TeamLeads : List<Participant>;
class Juniors : List<Participant>;
