FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-arm64v8 AS base
WORKDIR /app



FROM mcr.microsoft.com/dotnet/sdk:5.0-focal-arm64v8 AS build
WORKDIR /src
COPY ["ShortcutsBotHost/ShortcutsBotHost.csproj", "ShortcutsBotHost/"]
RUN dotnet restore "ShortcutsBotHost/ShortcutsBotHost.csproj"
COPY . .
WORKDIR "/src/ShortcutsBotHost"
RUN dotnet build "ShortcutsBotHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShortcutsBotHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShortcutsBotHost.dll"]
