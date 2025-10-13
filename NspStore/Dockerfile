# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .

RUN dotnet restore NspStore.Web/NspStore.Web.csproj
RUN dotnet publish NspStore.Web/NspStore.Web.csproj -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "NspStore.Web.dll"]
