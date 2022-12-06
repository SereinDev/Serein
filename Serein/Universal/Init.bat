@echo off
echo.Copyright (C) 2022 Zaitonn. All Rights Reserved.
echo %cd%| findstr Universal >nul && (
    echo.%cd%
) || (
    cd ../Universal
    echo.%cd%
)
if exist ./Build_Info.txt echo.Local>Build_Info.txt & echo.%date% %time%>>Build_Info.txt & echo.%cd%>>Build_Info.txt & echo.%os%>>Build_Info.txt & echo.null>>Build_Info.txt
)
exit 0
