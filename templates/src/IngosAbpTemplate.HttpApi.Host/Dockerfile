#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/IngosAbpTemplate.HttpApi.Host/IngosAbpTemplate.HttpApi.Host.csproj", "src/IngosAbpTemplate.HttpApi.Host/"]
COPY ["src/IngosAbpTemplate.EntityFrameworkCore/IngosAbpTemplate.EntityFrameworkCore.csproj", "src/IngosAbpTemplate.EntityFrameworkCore/"]
COPY ["src/IngosAbpTemplate.Domain/IngosAbpTemplate.Domain.csproj", "src/IngosAbpTemplate.Domain/"]
COPY ["src/IngosAbpTemplate.Domain.Shared/IngosAbpTemplate.Domain.Shared.csproj", "src/IngosAbpTemplate.Domain.Shared/"]
COPY ["src/IngosAbpTemplate.Application/IngosAbpTemplate.Application.csproj", "src/IngosAbpTemplate.Application/"]
COPY ["src/IngosAbpTemplate.Application.Contracts/IngosAbpTemplate.Application.Contracts.csproj", "src/IngosAbpTemplate.Application.Contracts/"]
RUN dotnet restore "src/IngosAbpTemplate.HttpApi.Host/IngosAbpTemplate.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/IngosAbpTemplate.HttpApi.Host"
RUN dotnet build "IngosAbpTemplate.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IngosAbpTemplate.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IngosAbpTemplate.HttpApi.Host.dll"]