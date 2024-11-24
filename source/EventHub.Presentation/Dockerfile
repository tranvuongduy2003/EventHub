FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["source/EventHub.Presentation/EventHub.Presentation.csproj", "source/EventHub.Presentation/"]
COPY ["source/EventHub.Abstractions/EventHub.Abstractions.csproj", "source/EventHub.Abstractions/"]
COPY ["source/EventHub.Domain/EventHub.Domain.csproj", "source/EventHub.Domain/"]
COPY ["source/EventHub.Shared/EventHub.Shared.csproj", "source/EventHub.Shared/"]
COPY ["source/EventHub.Persistence/EventHub.Persistence.csproj", "source/EventHub.Persistence/"]
COPY ["source/EventHub.Application/EventHub.Application.csproj", "source/EventHub.Application/"]
COPY ["source/EventHub.Infrastructure/EventHub.Infrastructure.csproj", "source/EventHub.Infrastructure/"]
COPY ["source/EventHub.SignalR/EventHub.SignalR.csproj", "source/EventHub.SignalR/"]
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