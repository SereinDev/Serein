@echo off
chcp 65001

cd ../Universal
echo.local>buildinfo.info & echo.%date% %time%>>buildinfo.info & echo.null>>buildinfo.info
exit 0
