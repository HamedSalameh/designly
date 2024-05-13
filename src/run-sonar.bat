dotnet sonarscanner begin /k:"designly-server" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_42645f76a3d2b6176a5f217362d276d981391410" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build --no-incremental
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_42645f76a3d2b6176a5f217362d276d981391410"

