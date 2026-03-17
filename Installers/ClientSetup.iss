[Setup]
AppName=PartsManager Client
AppVersion=1.0
DefaultDirName={localappdata}\PartsManager\Client
DefaultGroupName=PartsManager Client
OutputDir=Output
OutputBaseFilename=PartsManager_Client_Setup
Compression=lzma
SolidCompression=yes

[Files]
; 指向編譯後的發佈路徑 (改為絕對路徑)
Source: "C:\SourceCode\CS\PartsManager\Installers\publish\client\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\零件管理系統"; Filename: "{app}\PartsManager.Client.exe"
Name: "{commondesktop}\零件管理系統"; Filename: "{app}\PartsManager.Client.exe"

[Code]
var
  ServerIPPage: TInputQueryWizardPage;

procedure InitializeWizard;
begin
  // 建立自定義頁面詢問 Server IP
  ServerIPPage := CreateInputQueryPage(wpSelectDir,
    '伺服器連線設定', '請輸入 API 伺服器的 IP 地址',
    '如果您不知道伺服器 IP，請聯繫系統管理員。預設為 localhost。');
  ServerIPPage.Add('伺服器 IP:', False);
  ServerIPPage.Values[0] := 'localhost';
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ConfigPath: string;
  ServerIP: string;
begin
  // 安裝完成後執行
  if CurStep = ssPostInstall then
  begin
    ConfigPath := ExpandConstant('{app}\config.ini');
    ServerIP := ServerIPPage.Values[0];
    
    // 如果 config.ini 不存在，建立一個基本的
    if not FileExists(ConfigPath) then
    begin
      SaveStringToFile(ConfigPath, '[Network]' + #13#10 + 'ServerIP=' + ServerIP + #13#10 + 'ServerPort=5000' + #13#10, False);
    end
    else
    begin
      // 自動更新 config.ini 中的 ServerIP
      SetIniString('Network', 'ServerIP', ServerIP, ConfigPath);
      SetIniString('Network', 'ServerPort', '5000', ConfigPath);
    end;
  end;
end;

[Run]
Filename: "{app}\PartsManager.Client.exe"; Description: "啟動零件管理系統"; Flags: nowait postinstall
