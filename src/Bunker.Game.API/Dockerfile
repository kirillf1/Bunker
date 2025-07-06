FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Bunker.Game.API/Bunker.Game.API.csproj", "src/Bunker.Game.API/"]
COPY ["src/Bunker.Application.Shared/Bunker.Application.Shared.csproj", "src/Bunker.Application.Shared/"]
COPY ["src/Bunker.Game.Application/Bunker.Game.Application.csproj", "src/Bunker.Game.Application/"]
COPY ["src/Bunker.Game.Domain/Bunker.Game.Domain.csproj", "src/Bunker.Game.Domain/"]
COPY ["src/Bunker.Domain.Shared/Bunker.Domain.Shared.csproj", "src/Bunker.Domain.Shared/"]
COPY ["src/Bunker.Game.Infrastructure/Bunker.Game.Infrastructure.csproj", "src/Bunker.Game.Infrastructure/"]
COPY ["src/Bunker.Infrastructure.Shared/Bunker.Infrastructure.Shared.csproj", "src/Bunker.Infrastructure.Shared/"]
COPY ["src/Bunker.MessageBus.Kafka/Bunker.MessageBus.Kafka.csproj", "src/Bunker.MessageBus.Kafka/"]
COPY ["src/Bunker.MessageBus.Abstractions/Bunker.MessageBus.Abstractions.csproj", "src/Bunker.MessageBus.Abstractions/"]
RUN dotnet restore "./src/Bunker.Game.API/Bunker.Game.API.csproj"
COPY . .
WORKDIR "/src/src/Bunker.Game.API"
RUN dotnet build "./Bunker.Game.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bunker.Game.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bunker.Game.API.dll"]