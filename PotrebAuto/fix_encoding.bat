@echo off
chcp 65001
for /r %%f in (*.cs) do (
    echo Processing %%f
    powershell -Command "Get-Content '%%f' -Encoding UTF8 | Set-Content '%%f' -Encoding UTF8"
)
for /r %%f in (*.xaml) do (
    echo Processing %%f
    powershell -Command "Get-Content '%%f' -Encoding UTF8 | Set-Content '%%f' -Encoding UTF8"
)
echo Done!
pause