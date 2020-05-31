#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# Setup NodeJs
RUN apt-get -qq update && \
    apt-get -qq install -y wget && \
    apt-get -qq install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get -qq install -y build-essential nodejs && \
    apt-get -qq install -y nginx
# End setup
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# Setup NodeJs
RUN apt-get -qq update && \
    apt-get -qq install -y wget && \
    apt-get -qq install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get -qq install -y build-essential nodejs && \
    apt-get -qq install -y nginx
# End setup
WORKDIR /src
COPY ["NewsMaker.Web/NewsMaker.Web.csproj", "NewsMaker.Web/"]
COPY ["Infrastructure/Infrastructure.EventBus/Infrastructure.EventBus.csproj", "Infrastructure/Infrastructure.EventBus/"]
COPY ["Infrastructure/Infrastructure.Data/Infrastructure.Data.csproj", "Infrastructure/Infrastructure.Data/"]
COPY ["Domain/Domain.Core/Domain.Core.csproj", "Domain/Domain.Core/"]
RUN dotnet restore "NewsMaker.Web/NewsMaker.Web.csproj"

COPY ["NewsMaker.Web/ClientApp/package.json", "NewsMaker.Web/ClientApp/"]
RUN cd NewsMaker.Web//ClientApp \
    && npm i --silent

COPY . .
WORKDIR "/src/NewsMaker.Web"
RUN dotnet build "NewsMaker.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsMaker.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsMaker.Web.dll"]
