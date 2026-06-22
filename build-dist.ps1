# Сборка портативного дистрибутива УБТЭ: Автоматизация.
# Собирает агрегатор и все подпрограммы (Release) и складывает в одну папку:
#   dist\UBTE_Auto\UBTE_Auto.exe + Programs\...
# Эту папку использует установщик (installer.iss); её же можно просто скопировать
# на рабочий ПК и запустить UBTE_Auto.exe.
# Запуск:  powershell -ExecutionPolicy Bypass -File build-dist.ps1   [-Config Release|Debug]
param([string]$Config = "Release")
$ErrorActionPreference = "Stop"
$root = $PSScriptRoot

# --- найти MSBuild ---
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$msb = & $vswhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" |
       Select-Object -First 1
if (-not $msb) { throw "MSBuild не найден (нужна Visual Studio 2022)." }

$subs = @("PotrebAuto", "PoteryGVS", "Form46Auto", "Form4.42Auto")

# --- 1. Подпрограммы (попадают в Programs\ через таргет CopyToPrograms) ---
foreach ($p in $subs) {
    Write-Host "Сборка $p ($Config)..."
    & $msb "$root\$p\$p.csproj" /t:Rebuild /p:Configuration=$Config "/p:SolutionDir=$root\$p\" /v:quiet /nologo /clp:ErrorsOnly
    if ($LASTEXITCODE -ne 0) { throw "Ошибка сборки $p" }
}

# --- 2. Агрегатор ---
Write-Host "Сборка UBTE_Auto ($Config)..."
& $msb "$root\UBTE_Auto\UBTE_Auto.csproj" /t:Rebuild /p:Configuration=$Config "/p:SolutionDir=$root\UBTE_Auto\" /v:quiet /nologo /clp:ErrorsOnly
if ($LASTEXITCODE -ne 0) { throw "Ошибка сборки UBTE_Auto" }

# --- 3. Сборка дистрибутива ---
$dist = "$root\dist\UBTE_Auto"
if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }
New-Item -ItemType Directory -Path $dist -Force | Out-Null

# выход агрегатора без отладочных и ClickOnce-артефактов
$skipExt = @(".pdb", ".application", ".manifest", ".pfx", ".xml")
Get-ChildItem "$root\UBTE_Auto\bin\$Config" -File |
    Where-Object { $skipExt -notcontains $_.Extension } |
    ForEach-Object { Copy-Item $_.FullName -Destination $dist }

# папка Programs рядом с exe
Copy-Item "$root\Programs" -Destination "$dist\Programs" -Recurse -Force

# чистим Programs от мусора (пользователю он не нужен)
Get-ChildItem "$dist\Programs" -Recurse -Directory -Filter "app.publish" |
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Get-ChildItem "$dist\Programs" -Recurse -File -Include *.pdb, *.xml, *.application, *.manifest |
    Remove-Item -Force -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "Готово. Дистрибутив: $dist"
