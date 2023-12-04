FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FineApp/FineApp.csproj", "FineApp/"]
RUN dotnet restore "FineApp/FineApp.csproj"
COPY . .
WORKDIR "/src/FineApp"
RUN dotnet build "FineApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FineApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FineApp.dll"]
