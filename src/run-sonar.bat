dotnet sonarscanner begin /k:"Designly-Server" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_06193e6a00109fe6916d7c34e363e218b8d02769" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build --no-incremental
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_06193e6a00109fe6916d7c34e363e218b8d02769"

