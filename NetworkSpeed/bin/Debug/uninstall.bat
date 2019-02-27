@echo off

%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /nologo /unregister %~dp0NetworkSpeed.dll

taskkill /f /im explorer.exe 
start explorer.exe 

pause