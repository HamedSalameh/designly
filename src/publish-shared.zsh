#!/bin/zsh

# Define your GitHub Personal Access Token (PAT)
GITHUB_TOKEN="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"

# Define the name and version for the package
PACKAGE_NAME="Designly.Shared"
PACKAGE_VERSION="1.0.0"

# Define the GitHub Package Registry URL
GITHUB_PKG_REGISTRY_URL="https://nuget.pkg.github.com/HamedSalameh/index.json"

# Build the project in RELEASE mode
dotnet build -c Release ./Infrastructure/Shared/Designly.Shared.csproj

# Pack the project into a NuGet package
dotnet pack -c Release ./Infrastructure/Shared/Designly.Shared.csproj -o ./nupkgs

# Push the NuGet package to GitHub Package Registry
for PACKAGE in ./nupkgs/$PACKAGE_NAME.$PACKAGE_VERSION.nupkg; do
    dotnet nuget push "$PACKAGE" --source $GITHUB_PKG_REGISTRY_URL --api-key $GITHUB_TOKEN
done

# Clean up the generated NuGet packages
rm -rf ./nupkgs
