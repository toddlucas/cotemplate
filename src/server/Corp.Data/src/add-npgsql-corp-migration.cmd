dotnet ef migrations add %1 --context CorpDbContext --project ..\..\Corp.Data.Npgsql\src --output-dir Migrations\Corp --startup-project ..\..\Corp.Web\src -- CorpDbProvider=Npgsql
