#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Slip/Slip.Service/Slip.Service.csproj", "src/Services/Slip/Slip.Service/"]
COPY ["src/Shared/Shared.DAL.Postgres/Shared.DAL.Postgres.csproj", "src/Shared/Shared.DAL.Postgres/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "src/Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Types/Shared.Types.csproj", "src/Shared/Shared.Types/"]
COPY ["src/Shared/Shared.DAL.Transactions/Shared.DAL.Transactions.csproj", "src/Shared/Shared.DAL.Transactions/"]
COPY ["src/Shared/Shared.Cqrs/Shared.Cqrs.csproj", "src/Shared/Shared.Cqrs/"]
COPY ["src/Shared/Shared.Grpc/Shared.Grpc.csproj", "src/Shared/Shared.Grpc/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Web/Shared.Web.csproj", "src/Shared/Shared.Web/"]
COPY ["src/Shared/Shared.MessageBroker.Cqrs/Shared.MessageBroker.Cqrs.csproj", "src/Shared/Shared.MessageBroker.Cqrs/"]
COPY ["src/Shared/Shared.MessageBroker/Shared.MessageBroker.csproj", "src/Shared/Shared.MessageBroker/"]
COPY ["src/Shared/Shared.RabbitMq/Shared.RabbitMq.csproj", "src/Shared/Shared.RabbitMq/"]
RUN dotnet restore "src/Services/Slip/Slip.Service/Slip.Service.csproj"
COPY . .
WORKDIR "/src/src/Services/Slip/Slip.Service"
RUN dotnet build "Slip.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Slip.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slip.Service.dll"]