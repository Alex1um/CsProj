static class DictExtensions
{
    public static void Print<T, V>(this Dictionary<T, V> dict)
    {
        var result_string = "{\n";
        foreach (var (key, value) in dict)
        {
            result_string += $"\t{key}: {value}\n";
        }
        result_string += "}";
        Console.WriteLine(result_string);
    }
}