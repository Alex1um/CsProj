namespace HackatonService.src.ObjectOriented.Extensions;
using HackatonService.src.ObjectOriented.Participants;
using System.Text;

static class DictExtensions
{
    public static void PreetyPrint<T, V>(this Dictionary<T, V> dict) where T : notnull
    {
        var sb = new StringBuilder();
        sb.Append('{');
        foreach (var (key, value) in dict)
        {
            sb.Append('\t');
            sb.Append(key);
            sb.Append(" : ");
            sb.Append(value);
            sb.Append('\n');
        }
        sb.Append('}');
        Console.WriteLine(sb.ToString());
    }

    public static void Print<V>(this Dictionary<(Teamlead, Junior), V> dict)
    {
        var sb = new StringBuilder("{\n");
        foreach (var ((teamlead, junior), value) in dict)
        {
            sb.Append($"\t{teamlead}: {junior} = {value}\n");
        }
        sb.Append('}');
        Console.WriteLine(sb.ToString());
    }
}