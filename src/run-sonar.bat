dotnet sonarscanner begin /k:"designly" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_23483e3aebc2f11081b9bac2b9b83ad1cc185add" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_23483e3aebc2f11081b9bac2b9b83ad1cc185add"