cd src
dotnet restore
dotnet build MyLanguages.Core
msbuild MyLanguages.Wpf/MyLanguages.Wpf.csproj -p:Configuration=Release
pause