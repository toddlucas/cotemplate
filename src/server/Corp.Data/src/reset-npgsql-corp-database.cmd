psql -U postgres -a -c "DROP DATABASE IF EXISTS templateplaceholder.1"
rd /q /s ..\..\Corp.Data.Npgsql\src\Migrations\Corp
call add-npgsql-templateplaceholder.1-migration Initial
call update-npgsql-templateplaceholder.1-database
