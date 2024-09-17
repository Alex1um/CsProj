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
    
    public static void Print<V>(this Dictionary<(Teamlead, Junior), V> dict)
    {
        var result_string = "{\n";
        foreach (var ((teamlead, junior), value) in dict)
        {
            result_string += $"\t{teamlead}: {junior} = {value}\n";
        }
        result_string += "}";
        Console.WriteLine(result_string);
    }
}