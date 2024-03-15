FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5048

ENV ASPNETCORE_URLS=http://+:5048

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MinesWeeper.csproj", "./"]
RUN dotnet restore "MinesWeeper.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MinesWeeper.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MinesWeeper.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinesWeeper.dll"]
