@echo off
cd /d "C:\Users\dell\Downloads\CapG\Sprint_Project\Hospital_Management"
echo.
echo ========== GIT PULL ==========
echo Pulling remote changes from Siddharth branch...
git pull origin Siddharth --no-rebase
if %ERRORLEVEL% neq 0 (
    echo Pull failed! Check git status and resolve conflicts.
    pause
    exit /b 1
)
echo.
echo ========== GIT STATUS ==========
git status
echo.
echo ========== GIT PUSH ==========
echo Pushing local changes to Siddharth branch...
git push origin Siddharth
if %ERRORLEVEL% neq 0 (
    echo Push failed!
    pause
    exit /b 1
)
echo.
echo ========== SUCCESS ==========
echo Changes pushed successfully!
echo.
pause
