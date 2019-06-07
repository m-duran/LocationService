FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env

WORKDIR /app

# Copy csproj and restore 
COPY src/StatlerWaldorfCorp.LocationService/*.csproj ./
RUN dotnet restore

# Copy everything else
COPY src/StatlerWaldorfCorp.LocationService/. ./
RUN dotnet publish -c Release -o out

# Build image runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
EXPOSE 8080
COPY --from=build-env /app/docker_entrypoint.sh .
COPY --from=build-env /app/out/ .

ENTRYPOINT [ "/bin/bash", "docker_entrypoint.sh" ]
