FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:16.6.2 AS node-builder
WORKDIR /web
COPY ["src/Keis.Web/ClientApp/package.json", "./"]
COPY ["src/Keis.Web/ClientApp/package-lock.json", "./"]
RUN npm install --omit=dev --force
COPY ["src/Keis.Web/ClientApp/", "./"]
RUN export NODE_OPTIONS=--openssl-legacy-provider npm build

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Keis.Web/Keis.Web.csproj", "src/Keis.Web/"]
COPY ["src/Keis.Infrastructure/Keis.Infrastructure.csproj", "src/Keis.Infrastructure/"]
COPY ["src/Keis.Model/Keis.Model.csproj", "src/Keis.Model/"]
COPY ["src/Keis.Data/Keis.Data.csproj", "src/Keis.Data/"]
RUN dotnet restore "src/Keis.Web/Keis.Web.csproj"
COPY . .
WORKDIR "/src/src/Keis.Web"
RUN dotnet build "Keis.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Keis.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node-builder /web/build ./wwwroot
ENTRYPOINT ["dotnet", "Keis.Web.dll"]