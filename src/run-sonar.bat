dotnet sonarscanner begin /k:"designly" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_0ecd39a3c090d2f3f73b6e14944c0651a4e3898c" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_0ecd39a3c090d2f3f73b6e14944c0651a4e3898c"