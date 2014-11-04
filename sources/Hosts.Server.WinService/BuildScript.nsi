Unicode true

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Junte Queue Server"
!define EXE_NAME "Queue.Hosts.Server.WinService.exe"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "Junte"
!define PRODUCT_WEB_SITE "http://junte.ru/"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${EXE_NAME}"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define NET_FRAMEWORK_DOWNLOAD_URL "http://download.microsoft.com/download/B/A/4/BA4A7E71-2906-4B2D-A0E1-80CF16844F5F/dotNetFx45_Full_setup.exe"

; MUI 1.67 compatible ------
!include "MUI.nsh"
!include "StdUtils.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "Russian"

; MUI end ------

RequestExecutionLevel admin

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup_${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES32\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Function .onInit
  ReadRegStr $0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"
  ${If} $0 == ""
    goto done
  ${EndIf}

  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION  "Программа уже установлена. $\n$\nНажмите  'OK' для удаления или 'Отмена' чтобы отменить установку" IDOK uninst
  Abort

  uninst:
  ClearErrors
  ExecWait '"$INSTDIR\uninst.exe" _?=$INSTDIR' $0
  ${If} $0 != 0
    Abort
  ${EndIf}

  done:
FunctionEnd

Section -AdjustNetFramework
  ClearErrors
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"

  IfErrors NotDetected

  ${If} $0 >= 378389
     goto CompletedDotNet
  ${Else}
    NotDetected:
      DetailPrint ".NET Framework 4.5 Full не найден"
      MessageBox MB_YESNOCANCEL|MB_ICONEXCLAMATION \
             ".NET Framework 4.5 Full не установлен. Скачать с сайта www.microsoft.com?" /SD IDYES  IDYES DownloadDotNET IDNO CompletedDotNet
             goto CanceledDotNet
  ${EndIf}

DownloadDotNET:
  DetailPrint "Скачивание .NET Framework 4.5 Full..."
NSISDL::download "${NET_FRAMEWORK_DOWNLOAD_URL}" "$TEMP\dotnetfx.exe"
  Pop $0
  ${If} $0 == "cancel"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION "Скачивание отменено.  Продолжить установку?" IDYES CompletedDotNet IDNO CanceledDotNet
  ${ElseIf} $0 != "success"
    MessageBox MB_YESNO|MB_ICONEXCLAMATION "Скачивание не удалось:$\n$0$\n$\nПродолжить установку?" IDYES CompletedDotNet IDNO CanceledDotNet
  ${EndIf}

  DetailPrint "Установка .NET Framework 4.5 Full..."

  ${StdUtils.ExecShellWaitEx} $0 $1 '$TEMP\dotnetfx.exe' 'open' ''

  ${If} $0 == "error"
    Delete "$TEMP\dotnetfx.exe"
    goto CanceledDotNet
  ${Else}
    ${StdUtils.WaitForProcEx} $2 $1
    Delete "$TEMP\dotnetfx.exe"
    goto CompletedDotNet
  ${EndIf}

CanceledDotNet:
  Abort "Установка отменена пользователем"

CompletedDotNet:
  Pop $0
  Pop $1
  Pop $2
  Pop $3
  Pop $4
  Pop $5
  Pop $6
  Pop $7
    
SectionEnd

Section "CopyFiles"
  SetShellVarContext all
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  SetOverwrite try

  File "${EXE_NAME}"
  File "${EXE_NAME}.config"
  File "AutoMapper.dll"
  File "AutoMapper.Net4.dll"
  File "FluentNHibernate.dll"
  File "ICSharpCode.SharpZipLib.dll"
  File "Iesi.Collections.dll"
  File "Junte.Data.Common.dll"
  File "Junte.Data.NHibernate.dll"
  File "Junte.Parallel.Common.dll"
  File "Junte.WCF.Common.dll"
  File "log4net.dll"
  File "Microsoft.Practices.ServiceLocation.dll"
  File "Microsoft.Practices.Unity.dll"
  File "NHibernate.Caches.SysCache2.dll"
  File "NHibernate.dll"
  File "NHibernate.Mapping.Attributes.dll"
  File "NHibernate.Validator.dll"
  File "NPOI.dll"
  File "Queue.Common.dll"
  File "Queue.Model.Common.dll"
  File "Queue.Model.dll"
  File "Queue.Reports.dll"
  File "Queue.Server.dll"
  File "Queue.Services.Common.dll"
  File "Queue.Services.Contracts.dll"
  File "Queue.Services.DTO.dll"
  File "Queue.Services.Server.dll"
SectionEnd

Section -InstallService
  DetailPrint "Установка сервиса..."

  ExecWait 'sc create "${PRODUCT_NAME}" binPath= "$INSTDIR\${EXE_NAME}" DisplayName= "${PRODUCT_NAME}" start= auto' $0
  ${If} $0 != 0
    DetailPrint "Сервис не установлен."
    MessageBox MB_ICONINFORMATION|MB_OK "Не удалось установить сервис"
    Abort
  ${EndIf}

  DetailPrint "Сервис успешно установлен"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\AppMainExe.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\${EXE_NAME}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

Section -RunService
  ExecWait 'sc start "${PRODUCT_NAME}"' $0
  ${If} $0 != 0
    DetailPrint "Не удалось запустить сервис"
    MessageBox MB_ICONINFORMATION|MB_OK "Не удалось запустить службу. Ошибка: $0"
  ${EndIf}
SectionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "Удаление программы $(^Name) было успешно завершено."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Вы уверены в том, что желаете удалить $(^Name) и все компоненты программы?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  ExecWait 'sc stop "${PRODUCT_NAME}"'
  ExecWait 'sc delete "${PRODUCT_NAME}"'

  RMDir /r "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  
  SetAutoClose true
SectionEnd