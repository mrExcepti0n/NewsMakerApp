#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Comment.API/Comment.API.csproj", "Services/Comment.API/"]
RUN dotnet restore "Services/Comment.API/Comment.API.csproj"
COPY . .
WORKDIR "/src/Services/Comment.API"
RUN dotnet build "Comment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Comment.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Comment.API.dll"]