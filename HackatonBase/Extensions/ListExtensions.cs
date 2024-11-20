namespace HackatonBase.Extensions;

public static class ListExtensions
{

    static public List<T> GetShuffled<T>(this List<T> list)
    {
        var random = new Random();
        var arr = list.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            var j = random.Next(i, arr.Length);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        return [.. arr];
    }

    static public double GetHarmonicMean(this List<int> list)
    {
        var sum = 0.0;
        for (int i = 0; i < list.Count; i++)
        {
            sum += 1.0 / list[i];
        }
        return list.Count / sum;
    }

    static public void Print<T>(this List<T> list)
    {
        if (list.Count < 20)
        {
            Console.WriteLine("[" + string.Join(", ", list) + "]");
        }
        else
        {
            Console.WriteLine("[" + string.Join(",\n", list) + "]");
        }
    }

}