# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /src

ARG PACKAGE_TOKEN

# Get the dependencies

# Build the project layers
#  COPY ["./Clients.API/", "Clients.API/"]
#  COPY ["./Clients.Application/", "Clients.Application/"]
#  COPY ["./Clients.Domain/", "Clients.Domain/"]
#  COPY ["./Clients.Infrastructure/", "Clients.Infrastructure/"]
#  COPY ["./nuget.config", "nuget.config"]

COPY . .

# Restore nuget
RUN dotnet restore "Clients.API/clients.api.csproj" --configfile "nuget.config"
RUN dotnet restore "Clients.Application/Clients.Application.csproj" --configfile "nuget.config"
RUN dotnet restore "Clients.Domain/Clients.Domain.csproj" --configfile "nuget.config"
RUN dotnet restore "Clients.Infrastructure/Clients.Infrastructure.csproj" --configfile "nuget.config"


RUN find ./
# Build depedencies
RUN dotnet build "Clients.API/clients.api.csproj" -c Release -o /app/build

COPY . .
FROM build-env AS publish
WORKDIR ClientsService.API/
COPY . .
RUN dotnet publish "ClientsService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/build .

# Default web access port - this shall always be the last one
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "clients.api.dll"]