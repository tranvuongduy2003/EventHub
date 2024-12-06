FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["source/EventHub.Application/EventHub.Application.csproj", "source/EventHub.Application/"]
COPY ["source/EventHub.Domain/EventHub.Domain.csproj", "source/EventHub.Domain/"]
COPY ["source/EventHub.Domain.Shared/EventHub.Domain.Shared.csproj", "source/EventHub.Domain.Shared/"]
COPY ["source/EventHub.Infrastructure/EventHub.Infrastructure.csproj", "source/EventHub.Infrastructure/"]
COPY ["source/EventHub.Infrastructure.Persistence/EventHub.Infrastructure.Persistence.csproj", "source/EventHub.Infrastructure.Persistence/"]
COPY ["source/EventHub.Infrastructure.SignalR/EventHub.Infrastructure.SignalR.csproj", "source/EventHub.Infrastructure.SignalR/"]
COPY ["source/EventHub.Presentation/EventHub.Presentation.csproj", "source/EventHub.Presentation/"]
RUN dotnet restore "source/EventHub.Presentation/EventHub.Presentation.csproj"
COPY . .
WORKDIR "/src/source/EventHub.Presentation"
RUN dotnet build "EventHub.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventHub.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventHub.Presentation.dll"]