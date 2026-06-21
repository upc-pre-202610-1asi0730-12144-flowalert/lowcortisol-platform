FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LowCortisol.Platform.API/LowCortisol.Platform.API.csproj", "LowCortisol.Platform.API/"]
RUN dotnet restore "LowCortisol.Platform.API/LowCortisol.Platform.API.csproj"

COPY . .
RUN dotnet publish "LowCortisol.Platform.API/LowCortisol.Platform.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["sh", "-c", "dotnet LowCortisol.Platform.API.dll --urls http://0.0.0.0:${PORT:-8080}"]
