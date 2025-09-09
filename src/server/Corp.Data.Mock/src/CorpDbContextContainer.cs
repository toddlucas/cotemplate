using Corp.Data.Mock.Database;

namespace Corp.Data.Mock;

public class CorpDbContextContainer : DbContextContainer
{
    public CorpDbContextContainer() : base(new CorpSqliteDatabaseSet())
    {
    }

    /// <summary>
    /// Creates a container scope which includes the Sqlite database contexts.
    /// </summary>
    public CorpDbContextScope BeginScope()
    {
        if (DatabaseSet == null)
            throw new ObjectDisposedException("The databases have been disposed");

        return new CorpDbContextScope(DatabaseSet);
    }
}
