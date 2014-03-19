; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "FotoSite"
#define MyAppVersion "1.0"
;{app} - The application directory, DefaultDirName, which the user selects on the Select Destination Location page of the wizard.
; � ��� ����������� ��� ����������� ���������
#define WebDir "{app}\FotoSiteReally"
#define UtilDir "{app}\util"
#define DefaultFotoFolderName "C:\Users\�������������\Documents\��� �������\foto"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{3A0FB873-597A-440B-A135-6C9D16FEDE0C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppComments=Web ���� ������� ��� ��������� ���������� � �������� ��� ��������� �������� 
;AppVerName={#MyAppName} {#MyAppVersion}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename=FotoSiteSetup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
SetupLogging =yes
DisableFinishedPage=True

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Dirs]
Name: c:\logs; Permissions: everyone-full; Flags: uninsneveruninstall
Name: {#WebDir}\bin
Name: {#WebDir}\Account
Name: {#WebDir}\App_Data
Name: {#WebDir}\Content\themes\base\images
Name: {#WebDir}\Content\themes\base\minified\images
Name: {#WebDir}\Images
Name: {#WebDir}\Scripts\WebForms\MSAjax

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; IconFilename: {sys}\shell32.dll; IconIndex: 32

[CustomMessages]
russian.InstallDotNet =����� ���������� web ����� ������� ���������������       %n���������� ���������� .NET Framework 4.5.       %n%n��������� ����������!
russian.InstallIIS =����� ���������� web ����� ������� ���������������       %n���������� ���������� Misrosoft IIS.       %n%n��������� ����������!
russian.VirtualDirNotInstalled=��������� ������� ����������� ������� IIS ��� web-�����!      %n%n���������� ������� ��� �������� �������!
russian.NoLoadFile=�� ������� ��������� ����      %n%1      %n
russian.NoModifyConfig=�� ������� ��������� ��������� ������������!     %n%n������� ������� ��������� � ���� ������������      %n%1      %n
russian.NoStringToReplace=� ����� ������������ %n%1      %n�� ������� �� ������ ��������� ������ %2      %n
russian.RepalceFailed=��������� ������ ��� ��������� ������������:      %n%1        %n
russian.BadReplaceCmdLine=������������ ��������� ������ ��� ��������� ������������:      %n%1        %n

[UninstallDelete]
;WARNING! � ��������� ������� �������� �������� � ������� ����� ���� �������� � ����, ��� � ��������� ����� ASP.NET
;������ ����� � ����� ������������� ���� �� ��������������� (�� ������������ ��� �������� �������� w3wp.exe)
;�� �� � ���� ����� ��������, ����� ����� ������� ��������� ����� �������������.
;Name: {#WebDir}; Type: filesandordirs
Name: {app}; Type: dirifempty

[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
; �������� ��� ����� ������ � ������
Source: ".\util\replace.exe"; DestDir: "{#UtilDir}"; Flags: ignoreversion
; ��� recursesubdirs - ������������ ��� ��������
Source: "..\FotoSite\bin\*.dll"; DestDir: "{#WebDir}\bin"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.ico"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.aspx"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.ascx"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.asax"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.Master"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.css"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.png"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\FotoSite\*.js"; DestDir: "{#WebDir}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Web.config-�� ���, � ������ �������� ����� ������ � �����
Source: "..\FotoSite\Web.config"; DestDir: "{#WebDir}"; Flags: ignoreversion; AfterInstall: ModifyConfig('{#WebDir}\Web.config')
Source: "..\FotoSite\Account\Web.config"; DestDir: "{#WebDir}\Account"; Flags: ignoreversion
; ��������� ���� �����, ����� ��������� ��� ����
Source: "..\FotoSite\Bundle.config"; DestDir: "{#WebDir}"; Flags: ignoreversion; AfterInstall: CreateFotoSiteVirtualDir()

[Code]
var
  WebSiteNamePage: TInputQueryWizardPage;
  WebSiteName: string;

  FotoFolderNamePage: TInputDirWizardPage;
  FotoFolderName: string;

const
  IISServerName = 'localhost';
  IISServerNumber = '1';

// �������� ����� ��� ����� ����� Web �����
procedure CreateWebSiteNamePage;
begin
	// ����� ��� ����� ����� Web �����
	WebSiteNamePage := CreateInputQueryPage(wpSelectProgramGroup, 'Web ����', '��������� ����� Web �����',
		'��������� ��������� ��� Web ����� ���� ��� ���������� �� ����������:');

	// ���� ��� �����
	WebSiteNamePage.Add('��� �����:', False);

	// �������� �� ���������
	WebSiteNamePage.Values[0] := GetPreviousData('WebSiteName', 'FotoSite');
end;

// �������� ����� ��� ����� ����� �������� � ������������
procedure CreateFotoFolderNamePage;
begin
	// ����� ��� ����� ����� �������� � ������������
  // WebSiteNamePage.ID - ����� ����� �����
	FotoFolderNamePage := CreateInputDirPage(WebSiteNamePage.ID, 'Web ����', '��������� ����� ����� � ������������',
		'��������� ��� ����� � ������������, ������� ����� ���������� ����, ���� ��� ���������� �� ���������:', false, '');

	// ���� ��� �����
	FotoFolderNamePage.Add('������� � ������������:');

	// �������� �� ���������
	FotoFolderNamePage.Values[0] := GetPreviousData('FotoFolderName', '{#DefaultFotoFolderName}');
end;

// ���������� � ������������ �������������� ������� � �����������
procedure InitializeWizard;
begin
	CreateWebSiteNamePage();
  CreateFotoFolderNamePage();
end;

// ��������, ��� ���������� .NET Framework 4
function IsDotNet4Installed(): Boolean;
begin
	result := RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4');
	if not result then
	begin
		MsgBox( CustomMessage('InstallDotNet'), mbError, MB_OK );
	end;
end;

// ��������, ��� ���������� Microsoft IIS
function IsIISInstalled() : Boolean;
var
	IIS: Variant;
begin
  try
    IIS := CreateOleObject('IISNamespace');
	result:=true;
  except
    result:=false;
	MsgBox( CustomMessage('InstallIIS'), mbError, MB_OK );
  end;
end;

// �������� ����� �� ������ �������� �� �������������� �������
procedure CreateFotoSiteVirtualDir();
var
	IIS, WebSite, WebServer, WebRoot, VDir: Variant;
	ErrorCode: Integer;
  Success: Boolean;

begin
	Success:=true;
	// ����������� � IIS
	try
		IIS := CreateOleObject('IISNamespace');
	except
		Success:=false;
		MsgBox( CustomMessage('InstallIIS'), mbError, MB_OK );
	end;

	if Success then
	begin
		try
			// ����� ������
			WebSiteName := WebSiteNamePage.Values[0];
			WebSite := IIS.GetObject('IIsWebService', IISServerName + '/w3svc');
			WebServer := WebSite.GetObject('IIsWebServer', IISServerNumber);
			WebRoot := WebServer.GetObject('IIsWebVirtualDir', 'Root');

			// ������� ����������� �������, ������� ���������� ���������
			try
				WebRoot.Delete('IIsWebVirtualDir', WebSiteName);
				// ��������� ���������
				WebRoot.SetInfo();
			except
			end;

			// ������� ����������� �������
			VDir := WebRoot.Create('IIsWebVirtualDir', WebSiteName);
			VDir.AccessRead := True;
			VDir.AccessScript:=true;
			VDir.DefaultDoc:='default.aspx';
			VDir.AppFriendlyName := WebSiteName;
			VDir.Path := ExpandConstant('{#WebDir}');
			VDir.AppCreate(True);
			// ��������� ���������
			VDir.SetInfo();

			// ��������� ���� � ��������
      // �� ������� ������ ���� ������, � �� IIS �� ��������
      Sleep(2000);
			ShellExec( 'open', 'http://' + IISServerName + '/' + WebSiteName + '/', '', '', SW_SHOW, ewNoWait, ErrorCode );
			Success:=true;
		except
			MsgBox( CustomMessage('VirtualDirNotInstalled'), mbError, MB_OK );
			Success:=false;
		end;
	end;
end;

// �������� ������������ ��������
function DeleteFotoSiteVirtualDir() : Boolean;
var
	IIS, WebSite, WebServer, WebRoot: Variant;
begin
	result:=true;
	// ����������� � IIS
	try
		IIS := CreateOleObject('IISNamespace');
	except
		result:=false;
	end;

	if result then
	begin
		try
			// ����� ������
			WebSite := IIS.GetObject('IIsWebService', IISServerName + '/w3svc');
			WebServer := WebSite.GetObject('IIsWebServer', IISServerNumber);
			WebRoot := WebServer.GetObject('IIsWebVirtualDir', 'Root');

			// ������� ����������� �������
			try
				WebRoot.Delete('IIsWebVirtualDir', WebSiteName);
				// ��������� ���������
				WebRoot.SetInfo();
			except
			end;
			result:=true;
		except
			result:=false;
		end;
	end;
end;

// ������ ��������� � �������� �����
function ReplaceInFile(const filename:string; const strfrom:string; const strto:string ): Boolean;
var
  ResultCode: Integer;
  params: String;

begin
  params := '"' + filename + '" "' + strfrom + '" "' + strto + '"';
  Log('params: ' + params);

  if Exec(ExpandConstant('{#UtilDir}\replace.exe'), params, ExpandConstant('{#UtilDir}'), SW_SHOW,
     ewWaitUntilTerminated, ResultCode) then
  begin
    if ResultCode = 0 then
    begin
      result:=true;
    end else
    begin
      if ResultCode = 1 then
      begin
        result:=false;
        MsgBox( FmtMessage(CustomMessage('NoStringToReplace'), [filename,strfrom]), mbError, MB_OK );
      end else
      begin
        if ResultCode = -1 then
        begin
          result:=false;
          MsgBox( FmtMessage(CustomMessage('BadReplaceCmdLine'), [params]), mbError, MB_OK );
        end else
        begin
          result:=false;
        end
      end
    end
  end
  else begin
		result:=false;
		MsgBox( FmtMessage(CustomMessage('RepalceFailed'), [SysErrorMessage(ResultCode)]), mbError, MB_OK );
  end;

  if result=false then
  begin
    MsgBox( FmtMessage(CustomMessage('NoModifyConfig'), [filename]), mbError, MB_OK );
  end
end;

// ��������� ������ ������������, ���� ���� ������ ������ ��� �������
procedure ModifyConfig(const configfile:string);
begin
	configfile:=ExpandConstant(configfile);
  FotoFolderName:=FotoFolderNamePage.Values[0];

	if FotoFolderName <> '{#DefaultFotoFolderName}' then
	begin
		// ����������� ����� ������� � ������������
		// ��������� �� ���������, �.�. ��� �������������� �� ����� ������
		// � ������� ����������� ����� - ��� ���������� ��������

    // ������ �� ���������� ��������� ����� � �����, ���� �����
    // ��������, ������� ��� ������, �������� �� �����
    // ������ " �� ��������� ������
    StringChangeEx(FotoFolderName, '\', '/', True);

		ReplaceInFile(configfile, '{#DefaultFotoFolderName}', FotoFolderName);
	end;
end;

// �������� ����� ����������
procedure BeforeInstall();
begin
end;

// �������� ����� ���������
procedure PostInstall();
begin
end;

// �������� ����� ��������������
procedure BeforeUninstall();
begin
	WebSiteName := GetPreviousData('WebSiteName', 'FotoSite');
end;

// �������� ����� �������������
procedure PostUninstall();
begin
	DeleteFotoSiteVirtualDir();
end;

function InitializeSetup(): Boolean;
begin
	// ��������, ��� ����������� ��� ����������� ��
	result := IsDotNet4Installed() and IsIISInstalled();
end;

// �������� �� ��������� ����� ���������
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssInstall then //��������������� ����� ������� �����������
  begin
	BeforeInstall();
  end
  else
  begin
	  if CurStep = ssPostInstall then
	  begin
		PostInstall();
	  end
	  else
	  begin
		  if CurStep = ssDone then
		  begin
			//MsgBox( 'ssDone', mbConfirmation, MB_OK );
		  end;
	  end;
  end;
end;

// �������� �� ��������� ����� �������������
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then //��������������� ����� ������� �������������
  begin
	BeforeUninstall();
  end
  else
  begin
	  if CurUninstallStep = usPostUninstall then // ����� �������������, �� ����� ��������� ����������
	  begin
		PostUninstall();
	  end
	  else
	  begin
		  if CurUninstallStep = usDone then // ����� ���������� ���������
		  begin
			//MsgBox( 'usDone ', mbConfirmation, MB_OK );
		  end;
	  end;
  end;
end;

// ���������� ������ ����� �������� ����������� ��� ���� �����������
procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
	SetPreviousData(PreviousDataKey, 'WebSiteName', WebSiteNamePage.Values[0]);
	SetPreviousData(PreviousDataKey, 'FotoFolderName', FotoFolderNamePage.Values[0]);
end;

// ����������� ��������� ����� ���������� ������������
function NextButtonClick(CurPageID: Integer): Boolean;
begin
	result := true;
end;

// ���������� ���� ���� �� �������� ���������� � ���������
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  S := MemoDirInfo + NewLine;
  S := S + NewLine;

  S := S + '��� �����:' + NewLine;
  S := S + Space + WebSiteNamePage.Values[0] + NewLine;
  S := S + NewLine;
  
  S := S + '������� � ������������:' + NewLine + Space;
  S := S + Space + FotoFolderNamePage.Values[0] + NewLine;
  S := S + NewLine;

  Result := S;
end;

