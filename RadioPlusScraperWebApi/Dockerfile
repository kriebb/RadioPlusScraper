FROM microsoft/aspnet:windowsservercore
ARG source
WORKDIR /inetpub/wwwroot
COPY ${source:-obj/Docker/publish} .
