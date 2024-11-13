using Microsoft.EntityFrameworkCore;
using HackatonService.Participants;
using HackatonService;
using System.Reflection.Metadata;
namespace HackatonService.DB;

public class HackatonDbContext(DbContextOptions<HackatonDbContext> options) : DbContext(options)
{
    internal DbSet<HackatonScheme> Hachatons { get; set; }
    public DbSet<HackatonRunScheme> HachatonRuns { get; set; }
    public DbSet<Junior> Juniors { get; set; }
    public DbSet<Teamlead> Teamleads { get; set; }
    public DbSet<PreferenceScheme<Teamlead, Junior>> TeamleadLists { get; set; }
    public DbSet<PreferenceScheme<Junior, Teamlead>> JuniorLists { get; set; }
    public DbSet<TeamScheme<Teamlead, Junior>> Teams { get; set; }
}