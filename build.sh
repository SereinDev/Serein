#!/bin/bash
dotnet build ./Serein.sln -p:EnableWindowsTargeting=true
dotnet publish -f net6.0-windows --no-self-contained -p:PublishSingleFile=true -p:RuntimeIdentifier=win-x64 -p:IncludeContentInSingleFile=true