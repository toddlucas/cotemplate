psql -U postgres -a -c "DROP DATABASE IF EXISTS corp"
psql -U postgres -a -c "DROP USER IF EXISTS corp_user"
rd /q /s ..\..\Corp.Data.Npgsql\src\Migrations\Corp
call add-npgsql-corp-migration Initial
call add-npgsql-corp-migration EnableRowLevelSecurity

.\Scripts\replace-migration-methods.py --up .\Scripts\rls-up.txt --down .\Scripts\rls-down.txt --no-backup --file-pattern "..\..\Corp.Data.Npgsql\src\Migrations\Corp\*_EnableRowLevelSecurity.cs"

call update-npgsql-corp-database
psql -U postgres -a -c "ALTER ROLE corp_user WITH PASSWORD 'abc123'"
