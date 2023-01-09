#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Timeline_Service/Timeline_Service.csproj", "Timeline_Service/"]
RUN dotnet restore "Timeline_Service/Timeline_Service.csproj"
COPY . .
WORKDIR "/src/Timeline_Service"
RUN dotnet build "Timeline_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Timeline_Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Timeline_Service.dll"]
