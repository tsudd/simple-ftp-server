@echo off
echo Starting deploing
cd /D c:\Windows\Microsoft.NET\Framework64\v4.0.30319
InstallUtil.exe /u J:\BSUIRSTUFF\3SEM\Labs\ITP\FileWatcherService\FileWatcher\bin\Debug\FileWatcherService.exe
InstallUtil.exe J:\BSUIRSTUFF\3SEM\Labs\ITP\FileWatcherService\FileWatcher\bin\Debug\FileWatcherService.exe
net start FileWatcherByTsudd 
pause