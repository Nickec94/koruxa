FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

USER root

WORKDIR /src

COPY . .

RUN dotnet publish "SRC.DiscordBot/SRC.DiscordBot.csproj" \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "SRC.DiscordBot.dll"]