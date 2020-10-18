echo off
cd  %~dp0
powershell.exe -command  "$StartUp="$Env:USERPROFILE+'\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup';$punchclockPath = Resolve-Path '.\PunchClock.exe';New-Item -ItemType SymbolicLink -Path $StartUp -Name 'PunchClock.lnk' -Value $punchclockPath -force"
echo If there is an output displaying the LastWriteTime of the file PunchClock.lnk the punch clock has succesfully been added to the startup folder.
pause

