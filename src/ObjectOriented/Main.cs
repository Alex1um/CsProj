
namespace CsProj.src.ObjectOriented;
using CsProj.src.ObjectOriented.Participants;
using CsProj.src.ObjectOriented.Extensions;

static class Program
{
    const int TRIES = 1;
    public static void Main(string[] args)
    {
        var hackaton = new Hackaton("./CSHARP_2024_NSU/Teamleads20.csv", "./CSHARP_2024_NSU/Juniors20.csv");

        var meanHarmonicsSum = 0.0;
        for (int i = 0; i < TRIES; i++)
        {
            meanHarmonicsSum = hackaton.Run();
        }

        Console.WriteLine(meanHarmonicsSum / TRIES);
    }
}