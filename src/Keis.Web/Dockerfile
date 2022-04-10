#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node AS node-builder
WORKDIR /node
copy ./src/Keis.Web/ClientApp/package.json /node/package.json
RUN npm install 
COPY ./src/Keis.Web/ClientApp /node
RUN npm run build    

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Keis.Web/Keis.Web.csproj", "src/Keis.Web/"]
COPY ["src/Keis.Infrastructure/Keis.Infrastructure.csproj", "src/Keis.Infrastructure/"]
COPY ["src/Keis.Data/Keis.Data.csproj", "src/Keis.Data/"]
COPY ["src/Keis.Model/Keis.Model.csproj", "src/Keis.Model/"]
RUN dotnet restore "src/Keis.Web/Keis.Web.csproj"
COPY . .
WORKDIR "/src/src/Keis.Web"
RUN dotnet build "Keis.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Keis.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir /app/wwwroot
COPY --from=publish /app/publish .
COPY --from=node-builder /node/build ./wwwroot
ENTRYPOINT ["dotnet", "Keis.Web.dll"]