Write-Host "🧹 Cleaning NatureHelp project..." -ForegroundColor Cyan

if (Test-Path "./src/NatureHelp/ClientApp/nature-help") {
    Push-Location "./src/NatureHelp/ClientApp/nature-help"
    Write-Host "🧼 Cleaning Angular frontend..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue node_modules, dist, .angular
    Pop-Location
}

if (Test-Path "./src/NatureHelp") {
    Push-Location "./src/NatureHelp"
    Write-Host "🧼 Cleaning ASP.NET backend..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue bin, obj, .vs, logs, tmp, temp
    Pop-Location
}

Write-Host "🧼 Removing system and editor files..." -ForegroundColor Yellow
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue .DS_Store, Thumbs.db, .vscode, .idea

Write-Host "🧼 Removing sensitive configuration files..." -ForegroundColor Yellow
Remove-Item -Force -ErrorAction SilentlyContinue .env, serviceAccountKey.json, firebase.json, appsettings.Production.json

Write-Host "✅ Cleanup complete! Project ready for archiving." -ForegroundColor Green
