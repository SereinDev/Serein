dotnet publish src/Serein.Cli -r win-x64        -f net8.0-windows 
dotnet publish src/Serein.Cli -r linux-x64      -f net8.0         
dotnet publish src/Serein.Cli -r linux-arm      -f net8.0         
dotnet publish src/Serein.Cli -r linux-arm64    -f net8.0         
dotnet publish src/Serein.Cli -r osx-x64        -f net8.0         
dotnet publish src/Serein.Cli -r osx-arm64      -f net8.0         

dotnet publish src/Serein.Lite

dotnet publish src/Serein.Plus
