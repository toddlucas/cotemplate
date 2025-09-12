psql -U postgres -a -c "DROP DATABASE IF EXISTS corp"
rd /q /s ..\..\Corp.Data.Npgsql\src\Migrations\Corp
call add-npgsql-corp-migration Initial
call add-npgsql-corp-migration EnableRowLevelSecurity

replace-migration-methods.py --up-text "            RlsPolicyManager.EnableRls(migrationBuilder);" --down-text "            RlsPolicyManager.DisableRls(migrationBuilder);" --no-backup --file-pattern "..\..\Corp.Data.Npgsql\src\Migrations\Corp\*_EnableRowLevelSecurity.cs"

call update-npgsql-corp-database
