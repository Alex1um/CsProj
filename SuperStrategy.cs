namespace CsProj;

using Nsu.HackathonProblem.Contracts;

using Junior = int;
using Teamlead = int;

public class SuperStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
    {
        var juniorsWishlistsLists = juniorsWishlists.ToList();
        var teamLeadsWishlistsLists = teamLeadsWishlists.ToList();
        var juniorsList = juniors.ToList();
        var teamLeadsList = teamLeads.ToList();

        static bool TeamLeadPrefersJun1OverJun2(Junior[] prefer, Junior m, Junior m1)
        {
            return Array.IndexOf(prefer, m1) < Array.IndexOf(prefer, m);
        }
        var assignments = new Dictionary<Teamlead, Junior>();

        var freeJuniors = new HashSet<Junior>();

        int freeJuniorCount = teamLeads.Count();

        while (freeJuniorCount > 0)
        {
            Junior? firstFreeJuniorId = null;
            foreach (var jun in juniors)
            {
                if (!freeJuniors.Contains(jun.Id))
                {
                    firstFreeJuniorId = jun.Id;
                    break;
                }
            }
            if (firstFreeJuniorId == null)
            {
                break;
            }

            for (int i = 0; i < teamLeads.Count() && !freeJuniors.Contains(firstFreeJuniorId.Value); i++)
            {
                var firstFreeJuniorIndex = Array.IndexOf(juniorsWishlistsLists.Select(x => x.EmployeeId).ToArray(), firstFreeJuniorId.Value);
                var teamleadOfPreferenceId = juniorsWishlistsLists[firstFreeJuniorIndex].DesiredEmployees[i];

                if (!assignments.TryGetValue(teamleadOfPreferenceId, out Junior value))
                {
                    assignments.Add(teamleadOfPreferenceId, firstFreeJuniorId.Value);
                    freeJuniors.Add(firstFreeJuniorId.Value);
                    freeJuniorCount--;
                }
                else
                {
                    var currentAssigementId = value;

                    var teamleadOfPreferenceIndex = Array.IndexOf(teamLeadsWishlistsLists.Select(x => x.EmployeeId).ToArray(), teamleadOfPreferenceId);

                    if (TeamLeadPrefersJun1OverJun2(teamLeadsWishlistsLists[teamleadOfPreferenceIndex].DesiredEmployees, firstFreeJuniorId.Value, currentAssigementId) == false)
                    {
                        assignments[teamleadOfPreferenceId] = firstFreeJuniorId.Value;
                        freeJuniors.Add(currentAssigementId);
                        freeJuniors.Remove(firstFreeJuniorId.Value);
                    }
                }
            }
        }


        return assignments.Select(x => new Team(teamLeadsList.Where(y => y.Id == x.Key).First(), juniorsList.Where(y => y.Id == x.Value).First()));
    }
}
