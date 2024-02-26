#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt update \
    && apt install -y --no-install-recommends wget ca-certificates

RUN wget -O /usr/local/share/ca-certificates/aws-global-bundle.crt https://truststore.pki.rds.amazonaws.com/global/global-bundle.pem \
    && update-ca-certificates

WORKDIR /app
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY src/*/*.csproj ./
COPY Directory.Build.props ./

RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

RUN dotnet restore "WeArePlanet.SPV.Reports.Web/WeArePlanet.SPV.Reports.Web.csproj"

COPY src/. .

WORKDIR "/src/WeArePlanet.SPV.Reports.Web"
RUN dotnet build --no-restore "WeArePlanet.SPV.Reports.Web.csproj" -c Release

FROM build AS publish
RUN dotnet publish --no-build "WeArePlanet.SPV.Reports.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN useradd -r reports
USER reports

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WeArePlanet.SPV.Reports.Web.dll"]