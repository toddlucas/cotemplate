using Corp.Data.Mock.Database;

namespace Corp.Data.Mock;

public class CorpSqliteDatabaseSet : SqliteDatabaseSet
{
    private static readonly Dictionary<string, DbContextInfo> _infoMap = new()
    {
        [DatabaseNames.Corp] = new DbContextInfo(
            typeof(CorpDbContext),
            CorpDbContext.Create),

        [DatabaseNames.MyOther] = new DbContextInfo(
            typeof(MyOtherDbContext),
            MyOtherDbContext.Create),
    };

    public CorpSqliteDatabaseSet() : base(_infoMap)
    {
    }
}
