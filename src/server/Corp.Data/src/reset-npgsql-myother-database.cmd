psql -U postgres -a -c "DROP DATABASE IF EXISTS data"
rd /q /s ..\..\Corp.Data.Npgsql\src\Migrations\MyOther
call add-npgsql-data-migration Initial
call update-npgsql-data-database
