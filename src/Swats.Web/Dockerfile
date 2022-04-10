#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node AS node-builder
WORKDIR /node
copy ./src/Swats.Web/ClientApp/package.json /node/package.json
RUN npm install 
COPY ./src/Swats.Web/ClientApp /node
RUN npm run build    

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Swats.Web/Swats.Web.csproj", "src/Swats.Web/"]
COPY ["src/Swats.Infrastructure/Swats.Infrastructure.csproj", "src/Swats.Infrastructure/"]
COPY ["src/Swats.Data/Swats.Data.csproj", "src/Swats.Data/"]
COPY ["src/Swats.Model/Swats.Model.csproj", "src/Swats.Model/"]
RUN dotnet restore "src/Swats.Web/Swats.Web.csproj"
COPY . .
WORKDIR "/src/src/Swats.Web"
RUN dotnet build "Swats.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Swats.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir /app/wwwroot
COPY --from=publish /app/publish .
COPY --from=node-builder /node/build ./wwwroot
ENTRYPOINT ["dotnet", "Swats.Web.dll"]