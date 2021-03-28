#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

ENV ACCEPT_EULA Y
ENV DOTNET_ENVIRONMENT DockerDev
ENV AkkaConfig__SystemName CalculationSystem
ENV ConnectionStrings__DataPointDbContext Server=10.6.0.11;Database=volue_local;User Id=calculator;Password=Password123;

ARG BEFORE_SHELL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["src/Volue.Job.CalculationService/Volue.Job.CalculationService.csproj", "src/Volue.Job.CalculationService/"]
COPY ["src/Volue.Job.Messages/Volue.Job.Messages.csproj", "src/Volue.Job.Messages/"]
COPY ["src/Volue.Job.Persistance/Volue.Job.Persistance.csproj", "src/Volue.Job.Persistance/"]
COPY ["src/Volue.Job.Common/Volue.Job.Common.csproj", "src/Volue.Job.Common/"]
RUN dotnet restore "src/Volue.Job.CalculationService/Volue.Job.CalculationService.csproj"
COPY . .
WORKDIR "/src/src/Volue.Job.CalculationService"
RUN dotnet build "Volue.Job.CalculationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volue.Job.CalculationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 9000

# Build a shell script because the ENTRYPOINT command doesn't like using ENV
RUN echo "#!/bin/bash \n ${BEFORE_SHELL} \n dotnet Volue.Job.CalculationService.dll" > ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Run the generated shell script.
ENTRYPOINT ["./entrypoint.sh"]