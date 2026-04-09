cd /d %~dp0

set /P MOD_NAME=Mod Name:

powershell -NoProfile -NonInteractive -ExecutionPolicy Bypass -File "dev-items\initialize.ps1" -ModName "%MOD_NAME: =%"

