
del ..\..\Corp.Web\src\data.db
rd /q /s ..\..\Corp.Data.Sqlite\src\Migrations\MyOther
call add-sqlite-data-migration Initial
call update-sqlite-data-database
