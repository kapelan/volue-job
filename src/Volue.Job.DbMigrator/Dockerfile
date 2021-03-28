#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

ENV DOTNET_ENVIRONMENT=DockerDev

#CONNECTIONSTRING
ENV ConnectionStrings__OutboxDbContext="Server=10.6.0.11;Database=volue_local;User Id=sa;Password=SA_password123;"

ARG BEFORE_SHELL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /

COPY ["src/Volue.Job.DbMigrator/Volue.Job.DbMigrator.csproj", "src/Volue.Job.DbMigrator/"]
COPY ["src/Volue.Job.Persistance/Volue.Job.Persistance.csproj", "src/Volue.Job.Persistance/"]
RUN dotnet restore "src/Volue.Job.DbMigrator/Volue.Job.DbMigrator.csproj"

COPY . .
WORKDIR /src/Volue.Job.DbMigrator
RUN dotnet build "Volue.Job.DbMigrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Build a shell script because the ENTRYPOINT command doesn't like using ENV
RUN echo "#!/bin/bash \n ${BEFORE_SHELL} \n dotnet Volue.Job.DbMigrator.dll" > ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Run the generated shell script.
ENTRYPOINT ["./entrypoint.sh"]