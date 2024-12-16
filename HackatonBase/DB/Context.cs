using Microsoft.EntityFrameworkCore;
using HackatonBase.Participants;
using HackatonBase;
using System.Reflection.Metadata;
namespace HackatonBase.DB;

public class HackatonDbContext(DbContextOptions<HackatonDbContext> options) : DbContext(options)
{
    internal virtual DbSet<HackatonScheme> Hachatons { get; set; }
    public virtual DbSet<HackatonRunScheme> HachatonRuns { get; set; }
    public virtual DbSet<Junior> Juniors { get; set; }
    public virtual DbSet<Teamlead> Teamleads { get; set; }
    public virtual DbSet<PreferenceScheme<Teamlead, Junior>> TeamleadLists { get; set; }
    public virtual DbSet<PreferenceScheme<Junior, Teamlead>> JuniorLists { get; set; }
    public virtual DbSet<TeamScheme<Teamlead, Junior>> Teams { get; set; }
}