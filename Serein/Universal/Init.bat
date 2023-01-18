@echo off
chcp 65001
echo %cd%| findstr Universal >nul && (
    echo.%cd%
) || (
    cd ../Universal
    echo.%cd%
)
if exist ./buildinfo.info echo.local>buildinfo.info & echo.%date% %time%>>buildinfo.info & echo.null>>buildinfo.info
exit 0
