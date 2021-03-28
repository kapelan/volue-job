#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

ENV ACCEPT_EULA Y
ENV ASPNETCORE_ENVIRONMENT DockerDev

ENV WebApiAkkaConfig__SystemName CalculationServiceClientSystem
ENV WebApiAkkaConfig__CalculationServicePort 9000
ENV WebApiAkkaConfig__CalculationServiceHostname calculation-service
ENV WebApiAkkaConfig__ResponseTimeout 30
ENV WebApiAkkaConfig__CalculationServiceSystemName CalculationSystem
ENV ConnectionStrings__DataPointDbContext Server=10.6.0.11;Database=volue_local;User Id=webapi;Password=Password123;

ARG BEFORE_SHELL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["src/Volue.Job.WebApi/Volue.Job.WebApi.csproj", "src/Volue.Job.WebApi/"]
COPY ["src/Volue.Job.Messages/Volue.Job.Messages.csproj", "src/Volue.Job.Messages/"]
COPY ["src/Volue.Job.Persistance/Volue.Job.Persistance.csproj", "src/Volue.Job.Persistance/"]
COPY ["src/Volue.Job.Common/Volue.Job.Common.csproj", "src/Volue.Job.Common/"]
RUN dotnet restore "src/Volue.Job.WebApi/Volue.Job.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Volue.Job.WebApi"
RUN dotnet build "Volue.Job.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volue.Job.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000 50000 9001

# Build a shell script because the ENTRYPOINT command doesn't like using ENV
RUN echo "#!/bin/bash \n ${BEFORE_SHELL} \n dotnet Volue.Job.WebApi.dll" > ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Run the generated shell script.
ENTRYPOINT ["./entrypoint.sh"]