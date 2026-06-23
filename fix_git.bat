@echo off
cd /d "%~dp0"
SET "PATH=%PATH%;C:\Program Files\Git\cmd;C:\Program Files (x86)\Git\cmd;%LOCALAPPDATA%\Programs\Git\cmd"
git config --global --add safe.directory F:/fish/FishGame
echo [SUCCESS] Git safe directory fixed!
pause