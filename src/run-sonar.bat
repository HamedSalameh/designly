dotnet sonarscanner begin /k:"designly" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_9d3f03da8b9e8052496fdd394b7c47b91056ccc1" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_9d3f03da8b9e8052496fdd394b7c47b91056ccc1"