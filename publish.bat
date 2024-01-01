dotnet publish Serein.Cli -r win-x86        -f net6.0-windows 
dotnet publish Serein.Cli -r win-x64        -f net6.0-windows 
dotnet publish Serein.Cli -r linux-x64      -f net6.0         
dotnet publish Serein.Cli -r linux-arm      -f net6.0         
dotnet publish Serein.Cli -r linux-arm64    -f net6.0         
dotnet publish Serein.Cli -r osx-x64        -f net6.0         
dotnet publish Serein.Cli -r osx-arm64      -f net6.0         

dotnet publish Serein.Lite -r win-x86
dotnet publish Serein.Lite -r win-x64

dotnet publish Serein.Plus
