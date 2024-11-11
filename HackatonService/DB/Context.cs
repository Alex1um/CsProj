using Microsoft.EntityFrameworkCore;
using HackatonService.Participants;
using HackatonService.DB;
using HackatonService;
namespace HackatonService.DB;

public class HackatonDbContext(DbContextOptions<HackatonDbContext> options) : DbContext(options) {
    public DbSet<Junior> Juniors { get; set; }
    public DbSet<Teamlead> Teamleads { get; set; }
    public DbSet<PreferenceScheme<Teamlead, Junior>> TeamleadLists { get; set; }
    public DbSet<PreferenceScheme<Junior, Teamlead>> JuniorLists { get; set; }
    public DbSet<TeamScheme<Teamlead, Junior>> Teams { get; set; }
    public DbSet<TeamSatisfactionScheme<Teamlead, Junior>> SatisfactionIndex { get; set; }
    public DbSet<HarmonicMeanScheme> HarmonicMeans { get; set; }
}