#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ=America/Sao_Paulo

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GerenciadorDeCervejas.Api/GerenciadorDeCervejas.Api.csproj", "GerenciadorDeCervejas.Api/"]
COPY ["GerenciadorDeCervejas.Aplicacao/GerenciadorDeCervejas.Aplicacao.csproj", "GerenciadorDeCervejas.Aplicacao/"]
COPY ["GerenciadorDeCervejas.Dominio/GerenciadorDeCervejas.Dominio.csproj", "GerenciadorDeCervejas.Dominio/"]
COPY ["GerenciadorDeCervejas.Infra/GerenciadorDeCervejas.Infra.csproj", "GerenciadorDeCervejas.Infra/"]
COPY ["GerenciadorDeCervejas.Repositorio/GerenciadorDeCervejas.Repositorio.csproj", "GerenciadorDeCervejas.Repositorio/"]
RUN dotnet restore "GerenciadorDeCervejas.Api/GerenciadorDeCervejas.Api.csproj"
COPY . .
WORKDIR "/src/GerenciadorDeCervejas.Api"
RUN dotnet build "GerenciadorDeCervejas.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GerenciadorDeCervejas.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GerenciadorDeCervejas.Api.dll"]