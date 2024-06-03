dotnet sonarscanner begin /k:"Designly-Server" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_52c9a88dda814a1c5a77fc3479b3fa338e13e274" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml /d:sonar.exclusions="**/*Endpoint.cs,**/*Extensions.cs,**/*Extenstions.cs,**/Program.cs,**/*DbContext.cs,**/*UnitOfWork.cs,**/*Repository.cs,**/*Startup.cs"
dotnet build --no-incremental
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
dotnet sonarscanner end /d:sonar.token="sqp_52c9a88dda814a1c5a77fc3479b3fa338e13e274"

