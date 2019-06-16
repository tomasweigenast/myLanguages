cd src
dotnet restore MyLanguages.Core/MyLanguages.Core.csproj
dotnet build MyLanguages.Core/MyLanguages.Core.csproj
msbuild MyLanguages.Wpf/MyLanguages.Wpf.csproj -p:Configuration=Release
pause