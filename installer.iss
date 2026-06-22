; Установщик «УБТЭ Автоматизация» (Inno Setup 6).
; Перед компиляцией собрать дистрибутив:  powershell -ExecutionPolicy Bypass -File build-dist.ps1
; Компиляция:  "%LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe" installer.iss
; Результат:   dist\УБТЭ_Автоматизация_Setup.exe
; Ставится в папку пользователя (без прав администратора), создаёт ярлыки с иконкой.

#define AppName "УБТЭ Автоматизация"
#define AppVersion "1.0.0"
#define AppExe "UBTE_Auto.exe"

[Setup]
AppId={{A4F1C2E8-9B3D-4E7A-B6C1-2F8D9E0A1B23}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Оренбургский филиал АО ЭнергосбыТ Плюс
DefaultDirName={localappdata}\Programs\UBTE_Auto
DisableProgramGroupPage=yes
DisableDirPage=yes
PrivilegesRequired=lowest
OutputDir=dist
OutputBaseFilename=УБТЭ_Автоматизация_Setup
SetupIconFile=UBTE_Auto\AppData\Images\icon.ico
UninstallDisplayIcon={app}\{#AppExe}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "dist\UBTE_Auto\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs ignoreversion

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExe}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent
