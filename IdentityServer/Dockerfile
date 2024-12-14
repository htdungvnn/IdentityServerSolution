# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Stage 2: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file(s) and restore dependencies
COPY ["IdentityServer.csproj", "./"]
RUN dotnet restore "./IdentityServer.csproj"

# Copy the rest of the files and build the project
COPY . .
WORKDIR "/src"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

# Stage 3: Publish Stage
FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]