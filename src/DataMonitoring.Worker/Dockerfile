#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/DataMonitoring.Worker/DataMonitoring.Worker.csproj", "src/DataMonitoring.Worker/"]
COPY ["src/DataMonitoring.Core/DataMonitoring.Core.csproj", "src/DataMonitoring.Core/"]
RUN dotnet restore "src/DataMonitoring.Worker/DataMonitoring.Worker.csproj"
COPY . .
WORKDIR "/src/src/DataMonitoring.Worker"
RUN dotnet build "DataMonitoring.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataMonitoring.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataMonitoring.Worker.dll"]