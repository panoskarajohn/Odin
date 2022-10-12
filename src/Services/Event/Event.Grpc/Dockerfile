FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Event/Event.Grpc/Event.Grpc.csproj", "Event.Grpc/"]
RUN dotnet restore "src/Services/Event/Event.Grpc/Event.Grpc.csproj"
COPY . .
WORKDIR "/src/Event.Grpc"
RUN dotnet build "Event.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Event.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event.Grpc.dll"]
