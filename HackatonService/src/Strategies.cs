namespace HackatonService;

using HackatonService.Participants;

public abstract class ITeamBuildingStrategy
{
    public abstract AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadPrefStore,
        PreferencesStore<Junior, Teamlead> junPrefStore
        );
}

public class RandomTeamBuildingStrategy : ITeamBuildingStrategy {
    public override AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadPrefStore,
        PreferencesStore<Junior, Teamlead> junPrefStore
        )
    {
        var random = new Random();
        var juniorsShuffled = juniors.OrderBy(x => random.Next()).ToList();
        var teamleadsShuffled = teamleads.OrderBy(x => random.Next()).ToList();
        var resultList = new Dictionary<Teamlead, Junior>();
        for (int i = 0; i < juniorsShuffled.Count; i++)
        {
            resultList.Add(teamleadsShuffled[i], juniorsShuffled[i]);
        }
        return new AssignmentStore<Teamlead, Junior>(resultList);
    }
}
public class StableMarriageTeamBuildingStrategy : ITeamBuildingStrategy
{

    public override AssignmentStore<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        PreferencesStore<Teamlead, Junior> teamleadPrefStore,
        PreferencesStore<Junior, Teamlead> junPrefStore
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
                var teamleadOfPreference = junPrefStore[firstFreeJunior][i];

                if (!assignments.TryGetValue(teamleadOfPreference, out Junior? value))
                {
                    assignments.Add(teamleadOfPreference, firstFreeJunior);
                    freeJuniors.Add(firstFreeJunior);
                    freeJuniorCount--;
                }
                else
                {
                    var currentAssigement = value;

                    if (TeamLeadPrefersJun1OverJun2(teamleadPrefStore[teamleadOfPreference], firstFreeJunior, currentAssigement) == false)
                    {
                        assignments[teamleadOfPreference] = firstFreeJunior;
                        freeJuniors.Add(currentAssigement);
                        freeJuniors.Remove(firstFreeJunior);
                    }
                }
            }
        }

        return new AssignmentStore<Teamlead, Junior>(assignments);
    }
}