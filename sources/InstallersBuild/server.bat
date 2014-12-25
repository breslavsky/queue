@echo off

set MAKENSIS_PATH="C:\Program Files (x86)\NSIS\makensis.exe"
set EXE_NAME=Server_1.0.0.0.exe

md build
copy "..\Hosts.Server.WinService\bin\Release\Queue.Hosts.Server.WinService.exe" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Hosts.Server.WinService.exe.config" "build"
copy "..\Hosts.Server.WinService\bin\Release\AutoMapper.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\AutoMapper.Net4.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\FluentNHibernate.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\ICSharpCode.SharpZipLib.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Iesi.Collections.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Junte.Data.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Junte.Data.NHibernate.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Junte.Parallel.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Junte.WCF.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\log4net.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Microsoft.Practices.ServiceLocation.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Microsoft.Practices.Unity.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\NHibernate.Caches.SysCache2.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\NHibernate.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\NHibernate.Mapping.Attributes.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\NHibernate.Validator.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\NPOI.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Model.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Model.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Reports.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Server.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Services.Common.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Services.Contracts.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Services.DTO.dll" "build"
copy "..\Hosts.Server.WinService\bin\Release\Queue.Services.Server.dll" "build"

copy "..\Hosts.Server.WinForms\bin\Release\Queue.Hosts.Server.WinForms.exe" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Queue.Hosts.Server.WinForms.exe.config" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Junte.UI.WinForms.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Junte.UI.WinForms.NHibernate.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Microsoft.WindowsAPICodePack.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Microsoft.WindowsAPICodePack.Sensors.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Microsoft.WindowsAPICodePack.Shell.dll" "build"
copy "..\Hosts.Server.WinForms\bin\Release\Microsoft.WindowsAPICodePack.ShellExtensions.dll" "build"

copy "server.nsi" "build"

%MAKENSIS_PATH% "build\server.nsi"
copy "build\%EXE_NAME%" "%EXE_NAME%"
rd /S /Q "build"
 


