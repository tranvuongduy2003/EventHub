FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventHub.Shared/EventHub.Shared.csproj", "EventHub.Shared/"]
COPY ["EventHub.Domain/EventHub.Domain.csproj", "EventHub.Domain/"]
COPY ["EventHub.Infrastructor/EventHub.Infrastructor.csproj", "EventHub.Infrastructor/"]
COPY ["EventHub.Usecase/EventHub.Usecase.csproj", "EventHub.Usecase/"]
COPY ["EventHub.Presentation/EventHub.Presentation.csproj", "EventHub.Presentation/"]
RUN dotnet restore "EventHub.Presentation/EventHub.Presentation.csproj"
COPY . .
WORKDIR "/src/EventHub.Presentation"
RUN dotnet build "EventHub.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventHub.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventHub.Presentation.dll"]