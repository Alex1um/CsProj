namespace CsProj.src.ObjectOriented;

using CsProj.src.ObjectOriented.Participants;

public abstract class ITeamBuildingStrategy
{
    public abstract Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        );
}

public class RandomTeamBuildingStrategy : ITeamBuildingStrategy {
    public override Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        )
    {
        var random = new Random();
        var juniors_shuffled = juniors.OrderBy(x => random.Next()).ToList();
        var teamleads_shuffled = teamleads.OrderBy(x => random.Next()).ToList();
        var result_list = new Dictionary<Teamlead, Junior>();
        for (int i = 0; i < juniors_shuffled.Count; i++)
        {
            result_list.Add(teamleads_shuffled[i], juniors_shuffled[i]);
        }
        return result_list;
    }
}
public class StableMarriageTeamBuildingStrategy : ITeamBuildingStrategy
{

    public override Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        )
    {
        static bool TeamLeadPrefersJun1OverJun2(List<Junior> prefer, Junior m, Junior m1)
        {
            return prefer.IndexOf(m1) < prefer.IndexOf(m);
        }
        var assignments = new Dictionary<Teamlead, Junior>();

        var freeJuniors = new HashSet<Junior>();

        int freeJuniorCount = teamleads.Count;

        while (freeJuniorCount > 0)
        {
            Junior? firstFreeJunior = null;
            foreach (var jun in juniors)
            {
                if (!freeJuniors.Contains(jun))
                {
                    firstFreeJunior = jun;
                    break;
                }
            }
            if (firstFreeJunior == null)
            {
                break;
            }

            for (int i = 0; i < teamleads.Count && !freeJuniors.Contains(firstFreeJunior); i++)
            {
                var teamleadOfPreference = junLists[firstFreeJunior][i];

                if (!assignments.TryGetValue(teamleadOfPreference, out Junior? value))
                {
                    assignments.Add(teamleadOfPreference, firstFreeJunior);
                    freeJuniors.Add(firstFreeJunior);
                    freeJuniorCount--;
                }
                else
                {
                    var currentAssigement = value;

                    if (TeamLeadPrefersJun1OverJun2(teamleadLists[teamleadOfPreference], firstFreeJunior, currentAssigement) == false)
                    {
                        assignments[teamleadOfPreference] = firstFreeJunior;
                        freeJuniors.Add(currentAssigement);
                        freeJuniors.Remove(firstFreeJunior);
                    }
                }
            }
        }

        return assignments;
    }
}