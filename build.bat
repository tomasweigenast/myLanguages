cd src
dotnet restore
dotnet build
msbuild MyLanguages.Wpf/MyLanguages.Wpf.csproj -p:Configuration=Release
pause