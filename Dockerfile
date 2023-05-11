# Build

FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build

ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /source

COPY . .

RUN dotnet restore "./AmazonMockup.Web/AmazonMockup.Web.csproj" --disable-parallel

RUN dotnet publish "./AmazonMockup.Web/AmazonMockup.Web.csproj" -o /app --no-restore

# Serve

FROM mcr.microsoft.com/dotnet/aspnet:7.0.5

WORKDIR /app

COPY --from=build /app ./

ENTRYPOINT ["dotnet", "AmazonMockup.Web.dll"]