FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/PremiumService.API/PremiumService.API.csproj src/PremiumService.API/
COPY src/PremiumService.Application/PremiumService.Application.csproj src/PremiumService.Application/
COPY src/PremiumService.Domain/PremiumService.Domain.csproj src/PremiumService.Domain/
COPY src/PremiumService.Infrastructure/PremiumService.Infrastructure.csproj src/PremiumService.Infrastructure/
RUN dotnet restore src/PremiumService.API/PremiumService.API.csproj

COPY src/ src/
RUN dotnet publish src/PremiumService.API/PremiumService.API.csproj --configuration Release --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
	DOTNET_USE_POLLING_FILE_WATCHER=1
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet PremiumService.API.dll --urls http://0.0.0.0:${PORT:-8080}"]