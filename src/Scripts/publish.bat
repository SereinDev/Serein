dotnet publish src/Serein.Cli  -c Debug -r win-x64
dotnet publish src/Serein.Cli  -c Debug -r linux-x64
dotnet publish src/Serein.Cli  -c Debug -r linux-arm
dotnet publish src/Serein.Cli  -c Debug -r linux-arm64
dotnet publish src/Serein.Cli  -c Debug -r osx-x64
dotnet publish src/Serein.Cli  -c Debug -r osx-arm64
dotnet publish src/Serein.Lite -c Debug
dotnet publish src/Serein.Plus -c Debug
dotnet build src/Serein.Pro