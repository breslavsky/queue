@echo off

set MAKENSIS_PATH="C:\Program Files (x86)\NSIS\makensis.exe"
set EXE_NAME=Setup_1.0.0.0.exe

md build
copy "BuildScript.nsi"  "\bin\Release"

%MAKENSIS_PATH% "\bin\Release\BuildScript.nsi"
copy "\bin\Release\%EXE_NAME%" "%EXE_NAME%"
 


