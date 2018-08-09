FROM microsoft/aspnetcore:2.1 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.1 AS build
WORKDIR /src
COPY RadioPlusScraperCoreWebApp/RadioPlusScraperCoreWebApp.csproj RadioPlusScraperCoreWebApp/
RUN dotnet restore RadioPlusScraperCoreWebApp/RadioPlusScraperCoreWebApp.csproj
COPY . .
WORKDIR /src/RadioPlusScraperCoreWebApp
RUN dotnet build RadioPlusScraperCoreWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish RadioPlusScraperCoreWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RadioPlusScraperCoreWebApp.dll"]
