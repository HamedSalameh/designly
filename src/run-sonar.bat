dotnet sonarscanner begin /k:"designly-server" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_ad4dfd6071f85f28caa4152e0985e16aa1a02291" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml /d:sonar.exclusions="**/*Endpoint.cs,**/*Extensions.cs,**/*Extenstions.cs,**/Program.cs,**/*DbContext.cs,**/*UnitOfWork.cs,**/*Repository.cs,**/*Startup.cs"
dotnet build --no-incremental
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_ad4dfd6071f85f28caa4152e0985e16aa1a02291"

