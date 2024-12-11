using HackatonBase.Participants;

namespace HackatonBase.Extensions;

public static class MyExtensions
{
    // public static Junior ToJunior(this Participant junior)
    // {
    //     return junior as Junior;
    // }
    
    // public static Teamlead ToTeamlead(this Participant teamlead)
    // {
    //     return teamlead as Teamlead;
    // }

    // public static List<Teamlead> ToTeamleads(this List<Participant> teamleads)
    // {
    //     return teamleads.Select(x => x.ToTeamlead()).ToList();
    // }

    // public static List<Junior> ToJuniors(this List<Participant> juniors)
    // {
    //     return juniors.Select(x => x.ToJunior()).ToList();
    // }

    public static T ToSub<T>(this Participant junior) where T: Participant
    {
        return junior as T;
    }

    public static List<T> ToSub<T>(this List<Participant> participants) where T: Participant
    {
        return participants.Select(x => x as T).ToList();
    }
}
