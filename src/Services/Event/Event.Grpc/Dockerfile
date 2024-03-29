#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Event/Event.Grpc/Event.Grpc.csproj", "src/Services/Event/Event.Grpc/"]
COPY ["src/Shared/Shared.Grpc/Shared.Grpc.csproj", "src/Shared/Shared.Grpc/"]
COPY ["src/Shared/Shared.Types/Shared.Types.csproj", "src/Shared/Shared.Types/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Web/Shared.Web.csproj", "src/Shared/Shared.Web/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "src/Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Cqrs/Shared.Cqrs.csproj", "src/Shared/Shared.Cqrs/"]
COPY ["src/Shared/Shared.Metrics/Shared.Metrics.csproj", "src/Shared/Shared.Metrics/"]
COPY ["src/Shared/Shared.Prometheus/Shared.Prometheus.csproj", "src/Shared/Shared.Prometheus/"]
COPY ["src/Services/Event/Event.Application/Event.Application.csproj", "src/Services/Event/Event.Application/"]
COPY ["src/Services/Event/Event.Infrastructure/Event.Infrastructure.csproj", "src/Services/Event/Event.Infrastructure/"]
COPY ["src/Shared/Shared.Mongo/Shared.Mongo.csproj", "src/Shared/Shared.Mongo/"]
COPY ["src/Services/Event/Event.Core/Event.Core.csproj", "src/Services/Event/Event.Core/"]
COPY ["src/Shared/Shared.Domain/Shared.Domain.csproj", "src/Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.IdGenerator/Shared.IdGenerator.csproj", "src/Shared/Shared.IdGenerator/"]
RUN dotnet restore "src/Services/Event/Event.Grpc/Event.Grpc.csproj"
COPY . .
WORKDIR "/src/src/Services/Event/Event.Grpc"
RUN dotnet build "Event.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Event.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event.Grpc.dll"]