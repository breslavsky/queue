param([string]$PROJECT_NAME, [string]$DLL_FILE)

$BASE_PATH = "D:\projects\queue";
$PROJECT_PATH = "$BASE_PATH\sources\$PROJECT_NAME";
$PROJECT_FILE = "$PROJECT_PATH\$PROJECT_NAME.csproj";
$WIX_PROJECT_PATH = "$BASE_PATH\sources\Installer.$PROJECT_NAME";
$WIX_PROJECT_FILE = "$WIX_PROJECT_PATH\Installer.$PROJECT_NAME.wixproj";
$INSTALLERS_PATH = "$BASE_PATH\install\";

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8;

if (-Not (Get-Command "Invoke-MsBuild" -errorAction SilentlyContinue))
{
    (new-object Net.WebClient).DownloadString("https://raw.github.com/ligershark/psbuild/master/src/GetPSBuild.ps1") | iex;
}

Invoke-MsBuild $PROJECT_FILE -properties @{'Configuration'='Release'};


$ProductVersion = [Reflection.Assembly]::LoadFile("$PROJECT_PATH\Bin\Release\$DLL_FILE").GetName().Version.ToString();
$ProductCode = [guid]::NewGuid();


Invoke-MsBuild $WIX_PROJECT_FILE -properties @{'Configuration'='Release'; 'DefineConstants'="""ProductVersion=$ProductVersion;ProductCode=$ProductCode""" };

If (!(Test-Path $INSTALLERS_PATH)) 
{ 
   New-Item -Path $INSTALLERS_PATH -ItemType Directory;
}

Copy-Item "$WIX_PROJECT_PATH\bin\Release\ru-ru\Installer.$PROJECT_NAME.msi" "$INSTALLERS_PATH\$PROJECT_NAME $ProductVersion.msi";

Write-Output "Completed";
