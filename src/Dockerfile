# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala ve restore yap
COPY PTV_DEMO.sln .
COPY PTV.WebApi/*.csproj ./PTV.WebApi/
COPY PTV.Core/*.csproj ./PTV.Core/
COPY PTV.Application/*.csproj ./PTV.Application/
COPY PTV.Infrastructure/*.csproj ./PTV.Infrastructure/

RUN dotnet restore PTV.WebApi/*.csproj

# Uygulama kodlarını kopyala ve build yap
COPY . .
WORKDIR /app/PTV.WebApi
RUN dotnet publish -c Release -o /publish

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PTV.WebApi.dll"]
