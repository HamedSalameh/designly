dotnet sonarscanner begin /k:"designly" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_0181ed2cbb1c4c11a5e9b52bbe142a5534fa6cc2" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_0181ed2cbb1c4c11a5e9b52bbe142a5534fa6cc2"
