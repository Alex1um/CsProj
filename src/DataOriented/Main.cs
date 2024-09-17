
namespace CsProj.src.DataOriented;

static class Program
{
    const int TRIES = 1;
    public static void Main(string[] args)
    {

        var teamleads = CSVReader.Read<TeamLeads>("./CSHARP_2024_NSU/Teamleads20.csv");
        var juniors = CSVReader.Read<Juniors>("./CSHARP_2024_NSU/Juniors20.csv");

        var meanHarmonicsSum = 0.0;
        for (int i = 0; i < TRIES; i++)
        {
            // var junLists = new PreferList<Junior, Teamlead>(juniors, teamleads);
            // var teamleadLists = new PreferList<Teamlead, Junior>(teamleads, juniors);
            // var result_list = HR.AssignEmployeesAsStableMarriage(teamleads, juniors, teamleadLists, junLists);
            // var result_dict = HR.CalcSatisfactionIndex(teamleadLists, junLists, result_list);
            // result_dict.Print();
            // meanHarmonicsSum += HR.GetHarmonicMean(result_dict);
        }

        Console.WriteLine(meanHarmonicsSum / TRIES);
    }
}