FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/IngosAbpTemplate.API/IngosAbpTemplate.API.csproj", "IngosAbpTemplate.API/"]
RUN dotnet restore "src/IngosAbpTemplate.API/IngosAbpTemplate.API.csproj"
COPY . .
WORKDIR "/src/IngosAbpTemplate.API"
RUN dotnet build "IngosAbpTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IngosAbpTemplate.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IngosAbpTemplate.API.dll"]
