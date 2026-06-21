# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7242
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ds/", "ds/"]

RUN dotnet restore "ds/src/DS.Presentation/DS.Presentation.csproj"
COPY ["ds/", "ds/"]

# Миграции из ef-bundle
RUN dotnet tool install --global dotnet-ef --version 10.0
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN dotnet ef migrations bundle \
    --project "ds/src/DS.Infrastructure/DS.Infrastructure.csproj" \
    --startup-project "ds/src/DS.Presentation/DS.Presentation.csproj" \
    --output /app/efbundle \
    --configuration Release \
    --verbose \
    --self-contained

WORKDIR "/src/ds/src/DS.Presentation"
RUN dotnet build "./DS.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DS.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY --from=publish --chmod=755 /app/efbundle .

ENTRYPOINT ["dotnet", "DS.Presentation.dll"]