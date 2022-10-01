@echo off
echo.Copyright (C) 2022 Zaitonn. All Rights Reserved.
echo %cd%| findstr Universal >nul && (
    echo.%cd%
) || (
    cd ../Universal
    echo.%cd%
)
if %USERNAME% == "Administrator" do(
    echo.Local (Visual Studio Community 2022 With msbuild.exe)>Build_Info.txt
    echo.%date% %time%>>Build_Info.txt
    echo.%cd%>>Build_Info.txt
    echo.%os%>>Build_Info.txt
    echo.null>>Build_Info.txt
)
exit 0