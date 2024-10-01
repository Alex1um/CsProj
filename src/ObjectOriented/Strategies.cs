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
public class StableMarriageTeamBuildingStrategy : ITeamBuildingStrategy {

    public override Dictionary<Teamlead, Junior> BuildTeams(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        )
    {
        // This function returns true if woman 'w' prefers man 'm1' over man 'm'
        static bool wPrefersM1OverM<T, V>(List<V> prefer, T w, V m, V m1)
        {
            return prefer.IndexOf(m1) < prefer.IndexOf(m);
        }
        // Stores partner of women. This is our output array that
        // stores passing information.  The value of wPartner[i]
        // indicates the partner assigned to woman N+i.  Note that
        // the woman numbers between N and 2*N-1. The value -1
        // indicates that (N+i)'th woman is free
        var wPartner = new Dictionary<Teamlead, Junior>();

        // An array to store availability of men.  If mFree[i] is
        // false, then man 'i' is free, otherwise engaged.
        var mFree = new HashSet<Junior>();

        // Initialize all men and women as free
        // memset(wPartner, -1, sizeof(wPartner));
        // memset(mFree, false, sizeof(mFree));
        int freeCount = teamleads.Count;

        // While there are free men
        while (freeCount > 0)
        {
            // Pick the first free man (we could pick any)
            Junior m = new();
            foreach (var tmp in juniors)
            {
                if (!mFree.Contains(tmp))
                {
                    m = tmp;
                    break;
                }
            }

            // One by one go to all women according to m's preferences.
            // Here m is the picked free man
            for (int i = 0; i < teamleads.Count && !mFree.Contains(m); i++)
            {
                var w = junLists[m][i];

                // The woman of preference is free, w and m become
                // partners (Note that the partnership maybe changed
                // later). So we can say they are engaged not married
                if (!wPartner.TryGetValue(w, out Junior? value))
                {
                    wPartner.Add(w, m);
                    mFree.Add(m);
                    freeCount--;
                }
                else  // If w is not free
                {
                    // Find current engagement of w
                    var m1 = value;

                    // If w prefers m over her current engagement m1,
                    // then break the engagement between w and m1 and
                    // engage m with w.
                    if (wPrefersM1OverM(teamleadLists[w], w, m, m1) == false)
                    {
                        wPartner[w] = m;
                        mFree.Add(m1);
                        mFree.Remove(m);
                    }
                } // End of Else
            } // End of the for loop that goes to all women in m's list
        } // End of main while loop


        return wPartner;
    }
}