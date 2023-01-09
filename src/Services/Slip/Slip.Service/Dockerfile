FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Slip/Slip.Service/Slip.Service.csproj", "src/Services/Slip/Slip.Service/"]
RUN dotnet restore "src/Services/Slip/Slip.Service/Slip.Service.csproj"
COPY . .
WORKDIR "/src/src/Services/Slip/Slip.Service"
RUN dotnet build "Slip.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Slip.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slip.Service.dll"]
