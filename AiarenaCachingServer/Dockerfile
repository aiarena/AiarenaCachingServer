FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AiarenaCachingServer/AiarenaCachingServer.csproj", "AiarenaCachingServer/"]
RUN dotnet restore "AiarenaCachingServer/AiarenaCachingServer.csproj"
COPY . .
WORKDIR "/src/AiarenaCachingServer"
RUN dotnet build "AiarenaCachingServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AiarenaCachingServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir /app/config 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiarenaCachingServer.dll"]
