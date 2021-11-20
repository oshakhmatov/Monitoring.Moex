#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5433

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Monitoring.Moex.WebApi/Monitoring.Moex.WebApi.csproj", "Monitoring.Moex.WebApi/"]
RUN dotnet restore "Monitoring.Moex.WebApi/Monitoring.Moex.WebApi.csproj"
COPY . .
WORKDIR "/src/Monitoring.Moex.WebApi"
RUN dotnet build "Monitoring.Moex.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Monitoring.Moex.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitoring.Moex.WebApi.dll"]