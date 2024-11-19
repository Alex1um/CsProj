namespace HackatonService.Tests;
using HackatonService.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

public class DatabaseSqliteFixture : IDisposable
{
    public DatabaseSqliteFixture()
    {
        var _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        var _contextOptions = new DbContextOptionsBuilder<HackatonDbContext>()
            .UseSqlite(_connection)
            .Options;
        
        Db = _connection;
        context = new HackatonDbContext(_contextOptions);
    }

    public void ClearDatabase()
    {
        context.Database.ExecuteSqlRaw(@"DROP TABLE IF EXISTS public.""HachatonRuns"";

DROP TABLE IF EXISTS public.""Hachatons"";

DROP TABLE IF EXISTS public.""JuniorLists"";

DROP TABLE IF EXISTS public.""Juniors"";

DROP TABLE IF EXISTS public.""TeamleadLists"";

DROP TABLE IF EXISTS public.""Teamleads"";

DROP TABLE IF EXISTS public.""Teams"";");
    
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

    public SqliteConnection Db { get; private set; }
    public HackatonDbContext context { get; private set; }
}
