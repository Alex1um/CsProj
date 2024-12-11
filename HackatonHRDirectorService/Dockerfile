# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HackatonHRDirectorService/HackatonHRDirectorService.csproj", "HackatonHRDirectorService/"]
RUN dotnet restore "HackatonHRDirectorService/HackatonHRDirectorService.csproj"
COPY . .
WORKDIR "/src/HackatonHRDirectorService"
RUN dotnet build "HackatonHRDirectorService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HackatonHRDirectorService.csproj" -c Release -o /app/publish

# Create final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY "CSHARP_2024_NSU/Juniors5.csv" "/app/"
COPY "CSHARP_2024_NSU/Teamleads5.csv" "/app/"
ENTRYPOINT ["dotnet", "HackatonHRDirectorService.dll"]

