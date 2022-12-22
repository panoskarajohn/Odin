#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Identity/Identity.Api/Identity.Api.csproj", "src/Services/Identity/Identity.Api/"]
COPY ["src/Shared/Shared.IdGenerator/Shared.IdGenerator.csproj", "src/Shared/Shared.IdGenerator/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Web/Shared.Web.csproj", "src/Shared/Shared.Web/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "src/Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Types/Shared.Types.csproj", "src/Shared/Shared.Types/"]
COPY ["src/Shared/Shared.Cqrs/Shared.Cqrs.csproj", "src/Shared/Shared.Cqrs/"]
COPY ["src/Shared/Shared.Metrics/Shared.Metrics.csproj", "src/Shared/Shared.Metrics/"]
COPY ["src/Shared/Shared.Prometheus/Shared.Prometheus.csproj", "src/Shared/Shared.Prometheus/"]
COPY ["src/Shared/Shared.Swagger/Shared.Swagger.csproj", "src/Shared/Shared.Swagger/"]
COPY ["src/Services/Identity/Identity.Core/Identity.Core.csproj", "src/Services/Identity/Identity.Core/"]
COPY ["src/Shared/Shared.DAL.Postgres/Shared.DAL.Postgres.csproj", "src/Shared/Shared.DAL.Postgres/"]
COPY ["src/Shared/Shared.Jwt/Shared.Jwt.csproj", "src/Shared/Shared.Jwt/"]
COPY ["src/Shared/Shared.Security/Shared.Security.csproj", "src/Shared/Shared.Security/"]
RUN dotnet restore "src/Services/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]