# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

# Copy projects
COPY NatureHelp.sln ./NatureHelp/
COPY ./src/NatureHelp/NatureHelp.csproj ./src/NatureHelp/
COPY ./src/Application ./src/Application
COPY ./src/Domain ./src/Domain
COPY ./src/Infrastructure ./src/Infrastructure
COPY ./src/Shared ./src/Shared
COPY ./src/NatureHelp ./src/NatureHelp

# --- ❌ REMOVED THE BAD LINE HERE ❌ ---

# Restore and Publish
RUN dotnet restore "./src/NatureHelp/NatureHelp.csproj"
RUN dotnet publish "./src/NatureHelp/NatureHelp.csproj" -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS backend
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=backend-build /out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "NatureHelp.dll"]