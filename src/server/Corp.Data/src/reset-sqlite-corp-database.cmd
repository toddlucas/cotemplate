
del ..\..\Corp.Web\src\corp.db
rd /q /s ..\..\Corp.Data.Sqlite\src\Migrations\Corp
call add-sqlite-corp-migration Initial
call update-sqlite-corp-database
