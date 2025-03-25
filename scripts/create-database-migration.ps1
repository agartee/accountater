param(
  [Parameter(Mandatory = $true, HelpMessage = "Migration name")]
  [Alias("m")]
  [string]$migrationName
)

dotnet ef migrations add $migrationName `
  --startup-project ./src/Accountater.WebApp/ `
  --project ./src/Accountater.Persistence.SqlServer/ `
  --context AccountaterDbContext
