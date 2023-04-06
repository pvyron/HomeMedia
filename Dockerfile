FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY src/Presentation/HomeMedia.PiProxy/HomeMedia.PiProxy.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish *.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "HomeMedia.PiProxy.dll"]
