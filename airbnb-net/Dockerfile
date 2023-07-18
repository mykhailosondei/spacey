FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["airbnb-net/airbnb-net.csproj", "airbnb-net/"]
RUN dotnet restore "airbnb-net/airbnb-net.csproj"
COPY . .
WORKDIR "/src/airbnb-net"
RUN dotnet build "airbnb-net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "airbnb-net.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "airbnb-net.dll"]
