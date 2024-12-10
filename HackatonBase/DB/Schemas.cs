using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HackatonBase;
using HackatonBase.Participants;
using Microsoft.EntityFrameworkCore;

namespace HackatonBase.DB;

public class HackatonScheme
{
    public int Id { get; set; }
}

[PrimaryKey(nameof(Id))]
public class HackatonRunScheme
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(HackatonScheme))]
    public int HackatonId { get; set; }
    public double? mean { get; set; }

}

[PrimaryKey(nameof(HackatonId), nameof(UnitId))]
public class PreferenceScheme<T, V> where T : notnull, Participant where V : Participant
{

    [ForeignKey(nameof(HackatonScheme))]
    public int HackatonId { get; set; }

    [ForeignKey(nameof(T))]
    public int UnitId { get; set; }

    [ForeignKey(nameof(V))]
    public ICollection<int> Prefered { get; set; }
}

[PrimaryKey(nameof(HackatonRunId), nameof(TeamleadId), nameof(JuniorId))]
public class TeamScheme<T, V> where T : notnull where V : Participant
{

    [ForeignKey(nameof(HackatonRunScheme))]
    public int HackatonRunId { get; set; }

    [ForeignKey(nameof(T))]
    public int TeamleadId { get; set; }

    [ForeignKey(nameof(V))]
    public int JuniorId { get; set; }

    public double? Score { get; set; }
}

public static class SchemeConverters
{

    internal static PreferenceScheme<T, V>[] ToPreferencsScheme<T, V>(this PreferencesStore<T, V> assignmentStore, int hackatonId)
        where T : notnull, Participant where V : Participant
    {
        return assignmentStore.Select(assignment => new PreferenceScheme<T, V>
        {
            HackatonId = hackatonId,
            UnitId = assignment.Key.Id,
            Prefered = new List<int>(assignment.Value.Select(p => p.Id)), // assignment.Value,
        }).ToArray();
    }

    internal static TeamScheme<K, V>[] ToTeamsScheme<K, V>(this AssignmentStore<K, V> assignmentStore, int runId)
        where K : notnull, Participant where V : notnull, Participant
    {
        return assignmentStore.Select(assignment => new TeamScheme<K, V>
        {
            HackatonRunId = runId,
            TeamleadId = assignment.Key.Id,
            JuniorId = assignment.Value.Id,
        }).ToArray();
    }

}