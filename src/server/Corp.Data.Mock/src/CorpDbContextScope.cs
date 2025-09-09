using Corp.Data.Mock.Database;

namespace Corp.Data.Mock;

public class CorpDbContextScope : DbContextScope
{
    public CorpDbContextScope(SqliteDatabaseSet dbs) : base(dbs)
    {
    }

    internal CorpDbContext Corp => (CorpDbContext)GetContext(DatabaseNames.Corp);
    internal MyOtherDbContext MyOther => (MyOtherDbContext)GetContext(DatabaseNames.MyOther);
}
