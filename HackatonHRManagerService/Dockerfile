# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HackatonHRManagerService/HackatonHRManagerService.csproj", "HackatonHRManagerService/"]
RUN dotnet restore "HackatonHRManagerService/HackatonHRManagerService.csproj"
COPY . .
WORKDIR "/src/HackatonHRManagerService"
RUN dotnet build "HackatonHRManagerService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HackatonHRManagerService.csproj" -c Release -o /app/publish

# Create final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonHRManagerService.dll"]

