FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Event/Event.Api/Event.Api.csproj", "Event.Api/"]
RUN dotnet restore "Services/Event/Event.Api/Event.Api.csproj"
COPY . .
WORKDIR "/src/Event.Api"
RUN dotnet build "Event.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Event.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event.Api.dll"]
