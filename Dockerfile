FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Exchange.Core/Exchange.Core.csproj", "Exchange.Core/"]
COPY ["Exchange.Domain/Exchange.Domain.csproj", "Exchange.Domain/"]
COPY ["Exchange.CoinMarketCap/Exchange.CoinMarketCap.csproj", "Exchange.CoinMarketCap/"]
COPY ["Exchange.Database/Exchange.Database.csproj", "Exchange.Database/"]
COPY ["Exchange.TelegramBot/Exchange.TelegramBot.csproj", "Exchange.TelegramBot/"]
COPY ["Exchange.Web/Exchange.Web.csproj", "Exchange.Web/"]

RUN dotnet restore "Exchange.Web/Exchange.Web.csproj"
COPY . .  
WORKDIR "/src/Exchange.Web"
RUN dotnet build "Exchange.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Exchange.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Exchange.Web.dll"]
