#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/HomeMedia.PiProxy/HomeMedia.PiProxy.csproj", "src/Presentation/HomeMedia.PiProxy/"]
COPY ["src/Application/HomeMedia.Application/HomeMedia.Application.csproj", "src/Application/HomeMedia.Application/"]
COPY ["src/Domain/HomeMedia.Models/HomeMedia.Models.csproj", "src/Domain/HomeMedia.Models/"]
COPY ["src/Infrastructure/HomeMedia.Infrastructure/HomeMedia.Infrastructure.csproj", "src/Infrastructure/HomeMedia.Infrastructure/"]
COPY ["src/Presentation/HomeMedia.Contracts/HomeMedia.Contracts.csproj", "src/Presentation/HomeMedia.Contracts/"]
RUN dotnet restore "src/Presentation/HomeMedia.PiProxy/HomeMedia.PiProxy.csproj"
COPY . .
WORKDIR "/src/src/Presentation/HomeMedia.PiProxy"
RUN dotnet build "HomeMedia.PiProxy.csproj" -c Release -o /app/build

ENV ASPNETCORE_URLS=https://+:443;http://+:80

FROM build AS publish
RUN dotnet publish "HomeMedia.PiProxy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeMedia.PiProxy.dll"]