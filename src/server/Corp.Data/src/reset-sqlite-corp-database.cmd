
del ..\..\Corp.Web\src\templateplaceholder.1.db
rd /q /s ..\..\Corp.Data.Sqlite\src\Migrations\Corp
call add-sqlite-templateplaceholder.1-migration Initial
call update-sqlite-templateplaceholder.1-database
