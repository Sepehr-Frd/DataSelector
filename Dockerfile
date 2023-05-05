# Build

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:latest AS build

ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /source

COPY . .

RUN dotnet restore "./AmazonMockup.Web/AmazonMockup.Web.csproj" --disable-parallel

RUN dotnet publish "./AmazonMockup.Web/AmazonMockup.Web.csproj" -o /app --no-restore

# Serve

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:latest

WORKDIR /app

COPY --from=build /app ./

ENTRYPOINT ["dotnet", "AmazonMockup.Web.dll"]