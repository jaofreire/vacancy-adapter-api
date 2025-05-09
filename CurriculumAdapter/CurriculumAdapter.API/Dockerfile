# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CurriculumAdapter.API/CurriculumAdapter.API.csproj", "CurriculumAdapter.API/"]
RUN dotnet restore "./CurriculumAdapter.API/CurriculumAdapter.API.csproj"
COPY . .
WORKDIR "/src/CurriculumAdapter.API"
RUN dotnet build "./CurriculumAdapter.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CurriculumAdapter.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS certs
RUN mkdir /https-certs
RUN dotnet dev-certs https -ep /https-certs/aspnetcore.pfx -p iqwdf93jr9023njrm0923mf2qom3

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=certs /https-certs /https-certs

ENTRYPOINT ["dotnet", "CurriculumAdapter.API.dll"]