# Базовый образ для запуска (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Образ для сборки (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["NspStore.Web/NspStore.Web.csproj", "NspStore.Web/"]
COPY ["NspStore.Application/NspStore.Application.csproj", "NspStore.Application/"]
COPY ["NspStore.Infrastructure/NspStore.Infrastructure.csproj", "NspStore.Infrastructure/"]
RUN dotnet restore "NspStore.Web/NspStore.Web.csproj"

# Копируем всё остальное и собираем
COPY . .
WORKDIR "/src/NspStore.Web"
RUN dotnet build "NspStore.Web.csproj" -c Release -o /app/build

# Публикуем
FROM build AS publish
RUN dotnet publish "NspStore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы
COPY --from=publish /app/publish .

# Важно: appsettings.json попадёт в publish, если в csproj есть правило
# <Content Include="appsettings*.json" CopyToOutputDirectory="PreserveNewest" />

ENTRYPOINT ["dotnet", "NspStore.Web.dll"]
