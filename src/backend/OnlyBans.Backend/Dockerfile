# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY OnlyBans.Backend.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /publish .

EXPOSE 5107

ENV ASPNETCORE_URLS=http://+:5107

ENTRYPOINT ["dotnet", "MyProject.dll"]
