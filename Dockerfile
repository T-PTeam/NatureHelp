FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS backend-build
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

COPY NatureHelp.sln ./NatureHelp/
COPY ./src/NatureHelp/NatureHelp.csproj ./src/NatureHelp/
COPY ./src/Application ./src/Application
COPY ./src/Domain ./src/Domain
COPY ./src/Infrastructure ./src/Infrastructure
COPY ./src/Shared ./src/Shared
COPY ./src/NatureHelp ./src/NatureHelp

RUN dotnet restore "./src/NatureHelp/NatureHelp.csproj"
RUN dotnet publish "./src/NatureHelp/NatureHelp.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS backend
RUN apk add --no-cache curl icu-libs
WORKDIR /app
COPY --from=backend-build /out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "NatureHelp.dll"]
