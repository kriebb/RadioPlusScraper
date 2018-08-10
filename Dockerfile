# Build image
FROM microsoft/dotnet:2.1-sdk-alpine AS build 
WORKDIR /app
EXPOSE 80

WORKDIR /src
COPY . .
COPY RadioPlusScraperCoreWebApp/*.csproj ./RadioPlusScraperCoreWebApp/
COPY RadioPlusScraperNetStandard/*.csproj ./RadioPlusScraperNetStandard/
COPY Selenium.WebDriver.WaitExtensions.NetStandard/*.csproj ./Selenium.WebDriver.WaitExtensions.NetStandard/
RUN ls *.*
RUN dotnet restore RadioPlusScraper.sln

COPY . .
WORKDIR /src/Selenium.WebDriver.WaitExtensions.NetStandard
RUN dotnet build -c Release -o /app

WORKDIR /src/RadioPlusScraperNetStandard
RUN dotnet build -c Release -o /app

WORKDIR /src/RadioPlusScraperCoreWebApp
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM publish AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RadioPlusScraperCoreWebApp.dll"]

#App image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS runtime  
WORKDIR /app  
ENV ASPNETCORE_ENVIRONMENT Local  
ENTRYPOINT ["dotnet", "RadioPlusScraperCoreWebApp.dll"]  
COPY --from=publish /app .  