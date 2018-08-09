FROM microsoft/

ENV chocolateyUseWindowsCompression false

RUN powershell -Command \
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 ; \
    iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1')); \
    choco feature disable --name showDownloadProgress

RUN choco feature enable -n allowGlobalConfirmation
RUN choco install googlechrome -y
RUN choco install selenium-chrome-driver --version 2.37 -y


ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["C:\\app\\RadioPlusScraperSelfHost.exe"]

