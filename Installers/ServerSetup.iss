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
; 包含預設資料庫 (卸載時保留資料庫)
Source: "C:\SourceCode\CS\PartsManager\DB\Parts.db"; DestDir: "{app}\DB"; Flags: ignoreversion uninsneveruninstall
; 包含附件目錄結構
Source: "C:\SourceCode\CS\PartsManager\Attachments\*"; DestDir: "{app}\Attachments"; Flags: ignoreversion recursesubdirs

[Dirs]
Name: "{app}\DB"; Permissions: users-full
Name: "{app}\Attachments"; Permissions: users-full
Name: "{app}\logs"; Permissions: users-full

[Icons]
Name: "{group}\PartsManager API Server"; Filename: "{app}\PartsManager.Api.exe"

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
var
  ConfigPath: string;
  DBPath: string;
  AttachPath: string;
begin
  // 安裝完成後執行
  if CurStep = ssPostInstall then
  begin
    ConfigPath := ExpandConstant('{app}\config.ini');
    DBPath := ExpandConstant('{app}\DB\Parts.db');
    AttachPath := ExpandConstant('{app}\Attachments');
    
    // 自動更新 config.ini 中的 DefaultConnection
    // 格式: DefaultConnection=Data Source=C:\Path\To\DB\Parts.db
    SetIniString('ConnectionStrings', 'DefaultConnection', 'Data Source=' + DBPath, ConfigPath);
    
    // 自動更新 System:AttachmentPath
    SetIniString('System', 'AttachmentPath', AttachPath, ConfigPath);
  end;
end;

[Run]
; 安裝完後自動啟動 API 服務
Filename: "{app}\PartsManager.Api.exe"; Description: "啟動 PartsManager API 服務"; Flags: nowait postinstall
