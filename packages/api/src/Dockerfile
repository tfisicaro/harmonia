FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Harmonia.Api/Harmonia.Api.csproj", "Harmonia.Api/"]
RUN dotnet restore "Harmonia.Api/Harmonia.Api.csproj"
COPY . .
WORKDIR "/src/Harmonia.Api"
RUN dotnet build "Harmonia.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Harmonia.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Harmonia.Api.dll"]
