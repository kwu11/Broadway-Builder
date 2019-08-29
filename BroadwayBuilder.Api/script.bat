echo hello world
cd C:\GitHub Repos\Broadway-Builder\BroadwayBuilder.Api
msbuild BroadwayBuilder.Api.csproj /p:PublishProfile=FolderProfile /p:DeployOnBuild=true /p:Configuration=Release
pause