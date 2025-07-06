# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Bunker.ResultCreator.API/Bunker.ResultCreator.API.csproj", "src/Bunker.ResultCreator.API/"]
COPY ["src/Bunker.Application.Shared/Bunker.Application.Shared.csproj", "src/Bunker.Application.Shared/"]
COPY ["src/Bunker.MessageBus.Abstractions/Bunker.MessageBus.Abstractions.csproj", "src/Bunker.MessageBus.Abstractions/"]
COPY ["src/Bunker.Domain.Shared/Bunker.Domain.Shared.csproj", "src/Bunker.Domain.Shared/"]
COPY ["src/Bunker.Infrastructure.Shared/Bunker.Infrastructure.Shared.csproj", "src/Bunker.Infrastructure.Shared/"]
COPY ["src/Bunker.MessageBus.Kafka/Bunker.MessageBus.Kafka.csproj", "src/Bunker.MessageBus.Kafka/"]
RUN dotnet restore "./src/Bunker.ResultCreator.API/Bunker.ResultCreator.API.csproj"
COPY . .
WORKDIR "/src/src/Bunker.ResultCreator.API"
RUN dotnet build "./Bunker.ResultCreator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bunker.ResultCreator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bunker.ResultCreator.API.dll"]