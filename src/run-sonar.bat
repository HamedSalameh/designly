dotnet sonarscanner begin /k:"designly" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_4f5cd2eb83a1247220686f492ede824dfb33f1a0"
dotnet build
dotnet sonarscanner end /d:sonar.token="sqp_4f5cd2eb83a1247220686f492ede824dfb33f1a0"