namespace HackatonService.Tests;

using System.IO;

internal readonly struct FullPath
{
    public readonly string Path;

    public FullPath(string path) => Path = path;

    public static implicit operator string(FullPath path) => path.Path;

    public static implicit operator FullPath(string path) => new(path);
}

internal static class PathUtils
{  
    internal static readonly string ProjectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../../"));

    internal static FullPath GetFromProjectPath(string path)
    {
        
        return Path.Combine(ProjectPath, path);
    }
}