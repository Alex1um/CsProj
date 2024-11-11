using System.ComponentModel.DataAnnotations;
using HackatonService;
using HackatonService.Participants;

namespace HackatonService.DB;

public class HarmonicMeanScheme {

    [Key]
    internal Hackaton Hackaton { get; set; }
    public double mean { get; set; }
}

public class PreferenceScheme<T, V> where T : notnull where V : Participant {
    [Key]
    internal Hackaton Hackaton { get; set; }

    public T Unit { get; set; }
    public V Prefered { get; set; }
}

public class PreferencesStoreScheme<T, V> where T : notnull where V : Participant {
    [Key]
    internal Hackaton Hackaton { get; set; }

    public ISet<T> Units { get; set; }
    public ISet<V> Prefered { get; set; }

    public static implicit operator PreferencesStore<T, V>(PreferencesStoreScheme<T, V> scheme) {
        return new PreferencesStore<T, V>([.. scheme.Units], [.. scheme.Prefered]);
    }

}

public class AssignmentScheme<T, V> where T : notnull where V : Participant {
    [Key]
    internal Hackaton Hackaton { get; set; }
    public T Teamlead { get; set; }
    public V Junior { get; set; }
}

public class TeamScheme<T, V> where T : notnull where V : Participant {
    [Key]
    internal Hackaton Hackaton { get; set; }
    public T Teamlead { get; set; }
    public V Junior { get; set; }
}

public class TeamSatisfactionScheme<T, V> where T : notnull where V : Participant {
    [Key]
    public TeamScheme<T, V> Team { get; set; }
    public int Score { get; set; }
 }

public static class SchemeConverters {
    
    internal static AssignmentScheme<T, V> ToAssignmentScheme<T, V>(this Assignment<T, V> assignment, Hackaton hackaton)
        where T : notnull where V : Participant
    {
        return new AssignmentScheme<T, V> {
            Hackaton = hackaton,
            Teamlead = assignment.Teamlead,
            Junior = assignment.Junior
        };
    }

    internal static TeamScheme<T, V> ToTeamScheme<T, V>(this Assignment<T, V> assignment, Hackaton hackaton)
        where T : notnull where V : Participant
    {
        return new TeamScheme<T, V> {
            Hackaton = hackaton,
            Teamlead = assignment.Teamlead,
            Junior = assignment.Junior
        };
    }
}