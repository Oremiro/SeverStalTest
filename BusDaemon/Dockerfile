FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BusDaemon/BusDaemon.csproj", "BusDaemon/"]
RUN dotnet restore "BusDaemon/BusDaemon.csproj"
COPY . .
WORKDIR "/src/BusDaemon"
RUN dotnet build "BusDaemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusDaemon.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusDaemon.dll"]
