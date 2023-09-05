# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /src

# Get the dependencies

# Build the project layers
COPY ["./IdentityService.API/", "IdentityService.API/"]
COPY ["./IdentityService.Service/", "IdentityService.Service/"]
COPY ["./IdentityService.Application/", "IdentityService.Application/"]
COPY ["./IdentityService.Interfaces/", "IdentityService.Interfaces/"]

# Restore nuget
RUN dotnet restore "IdentityService.API/IdentityService.API.csproj"
RUN dotnet restore "IdentityService.Service/IdentityService.Service.csproj"
RUN dotnet restore "IdentityService.Application/IdentityService.Application.csproj"
RUN dotnet restore "IdentityService.Interfaces/IdentityService.Interfaces.csproj"

RUN find ./
# Build depedencies
RUN dotnet build "IdentityService.API/IdentityService.API.csproj" -c Release -o /app/build

COPY . .
FROM build-env AS publish
WORKDIR IdentityService.API/
COPY . .
RUN dotnet publish "IdentityService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/build .

# Default web access port - this shall always be the last one
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "IdentityService.API.dll"]
