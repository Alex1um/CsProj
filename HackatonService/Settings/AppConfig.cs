namespace HackatonService.Settings;

internal sealed class HackatonSettings
{
    public int RunsCount { get; set; }
}

internal sealed class DataSourceSettings
{
    public required string JuniorsListPath { get; set; }
    public required string TeamleadsListPath { get; set; }
}