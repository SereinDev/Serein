@echo off
echo %cd%| findstr Universal >nul && (
    echo.%cd%
) || (
    cd ../Universal
    echo.%cd%
)
if exist ./buildinfo echo.local>buildinfo & echo.%date% %time%>>buildinfo & echo.null>>buildinfo
exit 0
