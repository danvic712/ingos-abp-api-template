FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY ./ /app
WORKDIR /app/src/IngosAbpTemplate.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "IngosAbpTemplate.API.dll"]