#Build
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./src/StreamManager/*.csproj .
WORKDIR /app
RUN dotnet restore

COPY ./src/StreamManager/. .
WORKDIR /app
RUN dotnet publish -c Release -o out

#Create deployment container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "StreamManager.dll"]p