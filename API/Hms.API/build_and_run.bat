@echo off
cd /d "C:\Users\dell\Downloads\CapG\Sprint_Project\Hospital_Management\API\Hms.API"
echo.
echo ========== CLEANING ==========
dotnet clean
echo.
echo ========== RESTORING ==========
dotnet restore
echo.
echo ========== BUILDING ==========
dotnet build
if %ERRORLEVEL% neq 0 (
    echo Build failed!
    pause
    exit /b 1
)
echo.
echo ========== RUNNING API ==========
echo API will start on: https://localhost:5001
echo Swagger: https://localhost:5001/swagger/index.html
echo.
dotnet run
pause
