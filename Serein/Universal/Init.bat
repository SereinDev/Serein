chcp 65001
cd %cd%
echo.%cd%
if %USERNAME% == "Administrator" do(
    echo.Local (Visual Studio Community 2022 With msbuild.exe)>Build_Info.txt
    echo.%date% %time%>>Build_Info.txt
    echo.%cd%>>Build_Info.txt
    echo.%os%>>Build_Info.txt
    echo.null>>Build_Info.txt
)
exit 0