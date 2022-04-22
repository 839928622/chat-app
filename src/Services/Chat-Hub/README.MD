# Development
## Add Migration(s)
add migrations(s) by `dotnet ef`
`dotnet ef migrations add MigrationName -p Infrastructure  -s ChatHub.API -c ChatAppContext -o Persistence\Migrations`

or add migrations(s) in Visual Studio
`PM> Add-Migration Migration(s)Name -Project Infrastructure -StartupProject ChatHub.API -Context ChatAppContext -OutputDir "Persistence\Migrations"`