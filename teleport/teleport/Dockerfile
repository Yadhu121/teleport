# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore first (faster builds)
COPY *.csproj ./
RUN dotnet restore

# copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "teleport.dll"]

