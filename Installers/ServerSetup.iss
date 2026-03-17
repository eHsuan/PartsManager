[Setup]
AppName=PartsManager Server
AppVersion=1.0
DefaultDirName={commonpf}\PartsManager\Server
DefaultGroupName=PartsManager Server
OutputDir=Output
OutputBaseFilename=PartsManager_Server_Setup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Files]
; 指向編譯後的發佈路徑 (改為絕對路徑)
Source: "C:\SourceCode\CS\PartsManager\Installers\publish\server\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
; 包含預設資料庫 (如果不存在則安裝)
Source: "C:\SourceCode\CS\PartsManager\DB\Parts.db"; DestDir: "{app}\DB"; Flags: ignoreversion onlyifdestfileexists
; 包含附件目錄結構
Source: "C:\SourceCode\CS\PartsManager\Attachments\*"; DestDir: "{app}\Attachments"; Flags: ignoreversion recursesubdirs onlyifdestfileexists

[Icons]
Name: "{group}\PartsManager API Server"; Filename: "{app}\PartsManager.Api.exe"

[Run]
; 安裝完後自動啟動 API 服務
Filename: "{app}\PartsManager.Api.exe"; Description: "啟動 PartsManager API 服務"; Flags: nowait postinstall
