#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/WebApiGateway/WebApiGateway.csproj", "src/Services/WebApiGateway/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "src/Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Web/Shared.Web.csproj", "src/Shared/Shared.Web/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "src/Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Types/Shared.Types.csproj", "src/Shared/Shared.Types/"]
COPY ["src/Shared/Shared.Cqrs/Shared.Cqrs.csproj", "src/Shared/Shared.Cqrs/"]
RUN dotnet restore "src/Services/WebApiGateway/WebApiGateway.csproj"
COPY . .
WORKDIR "/src/src/Services/WebApiGateway"
RUN dotnet build "WebApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiGateway.dll"]