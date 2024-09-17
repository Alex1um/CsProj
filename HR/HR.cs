#define N 20

class HR
{
    public const int N = 20;

    public static Dictionary<(Teamlead, Junior), int> calcSatisfactionIndex(
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists,
        Dictionary<Teamlead, Junior> result_list
    )
    {
        var result_dict = new Dictionary<(Teamlead, Junior), int>();
        foreach (var (teamlead, junior) in result_list) {
            var team_lead_index = junLists[junior].IndexOf(teamlead);
            var team_lead_score = 20 - team_lead_index;
            var junior_index = teamleadLists[teamlead].IndexOf(junior);
            var junior_score = 20 - junior_index;
            result_dict[(teamlead, junior)] = team_lead_score + junior_score;
        }
        return result_dict;
    }

    public static double getHarmonicMean(Dictionary<(Teamlead, Junior), int> result_dict) {
        var sum = 0.0;
        foreach (var value in result_dict.Values) {
            sum += 1.0 / value;
        }
        return result_dict.Count / sum;
    }

    public static Dictionary<Teamlead, Junior> assignEmployees(
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

    public static Dictionary<Teamlead, Junior> assignEmployeesAsStableMarriage(
        List<Teamlead> teamleads,
        List<Junior> juniors,
        Dictionary<Teamlead, List<Junior>> teamleadLists,
        Dictionary<Junior, List<Teamlead>> junLists
        ) 
    {
        var result_list = new Dictionary<Teamlead, Junior>();

        // This function returns true if woman 'w' prefers man 'm1' over man 'm'
        static bool wPrefersM1OverM<T, V>(List<V> prefer, T w, V m, V m1)
        {
            return prefer.IndexOf(m1) < prefer.IndexOf(m);
            // Check if w prefers m over her current engagement m1
            for (int i = 0; i < N; i++)
            {
                // If m1 comes before m in list of w, then w prefers her
                // current engagement, don't do anything
                if (prefer[w, i] == m1)
                    return true;
        
                // If m comes before m1 in w's list, then free her current
                // engagement and engage her with m
                if (prefer[w, i] == m)
                return false;
            }
            return false;
        }
        // Teamlead = women, Junior = men

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
        int freeCount = N;
    
        // While there are free men
        while (freeCount > 0)
        {
            // Pick the first free man (we could pick any)
            int m;
            for (m = 0; m < N; m++)
                if (mFree[m] == false)
                    break;
    
            // One by one go to all women according to m's preferences.
            // Here m is the picked free man
            for (int i = 0; i < N && mFree[m] == false; i++)
            {
                int w = prefer[m][i];
    
                // The woman of preference is free, w and m become
                // partners (Note that the partnership maybe changed
                // later). So we can say they are engaged not married
                if (wPartner[w-N] == -1)
                {
                    wPartner[w-N] = m;
                    mFree[m] = true;
                    freeCount--;
                }
    
                else  // If w is not free
                {
                    // Find current engagement of w
                    int m1 = wPartner[w-N];
    
                    // If w prefers m over her current engagement m1,
                    // then break the engagement between w and m1 and
                    // engage m with w.
                    if (wPrefersM1OverM(prefer, w, m, m1) == false)
                    {
                        wPartner[w-N] = m;
                        mFree[m] = true;
                        mFree[m1] = false;
                    }
                } // End of Else
            } // End of the for loop that goes to all women in m's list
        } // End of main while loop
    
    
        return wPartner
    }
}