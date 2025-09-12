@echo off
echo Adding RLS migration for PostgreSQL...

cd /d "%~dp0"
dotnet ef migrations add EnableRowLevelSecurity --context CorpDbContext --project ../Corp.Data.Npgsql --startup-project ../../Corp.Web

echo Migration created. Please review and run update-database when ready.
pause
