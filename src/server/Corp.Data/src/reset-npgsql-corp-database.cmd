psql -U postgres -a -c "DROP DATABASE IF EXISTS corp"
rd /q /s ..\..\Corp.Data.Npgsql\src\Migrations\Corp
call add-npgsql-corp-migration Initial
call update-npgsql-corp-database
