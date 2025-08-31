dotnet ef migrations add %1 --context MyOtherDbContext --project ..\..\Corp.Data.Npgsql\src --output-dir Migrations\MyOther --startup-project ..\..\Corp.Web\src -- MyOtherDbProvider=Npgsql
