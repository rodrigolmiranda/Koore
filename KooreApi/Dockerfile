#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KooreApi/KooreApi.csproj", "KooreApi/"]
RUN dotnet restore "KooreApi/KooreApi.csproj"
COPY . .
WORKDIR "/src/KooreApi"
RUN dotnet build "KooreApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KooreApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KooreApi.dll"]