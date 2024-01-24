FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Domus.Api/Domus.Api.csproj", "Domus.Api/"]
RUN dotnet restore "Domus.Api/Domus.Api.csproj"
COPY . .
WORKDIR "/src/Domus.Api"
RUN dotnet build "Domus.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Domus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Domus.Api.dll"]
