De volgende informatie kan gebruikt worden om de docker container met HTTPS support te starten.
Voor de use case van deze POC is niet niet volledig getest.

Meer informatie: https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-5.0

docker run
-p 8080:80 -p 8081:443
-e ASPNETCORE_URLS="https://+;http://+"
-e ASPNETCORE_HTTPS_PORT=8081
-e ASPNETCORE_ENVIRONMENT=Development
-v $env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets
-v $env:USERPROFILE\.aspnet\https:/root/.aspnet/https/

==================

Docker-Compose

version: '3'
services:
foodapi:
build: .
ports: - "8080:80" - "8081:443"
environment:
ASPNETCORE_URLS: "https://+;http://+"
ASPNETCORE_HTTPS_PORT: "8081"
ASPNETCORE_ENVIRONMENT: "Development"
volumes: - ${APPDATA}\microsoft\UserSecrets\:/root/.microsoft/usersecrets - ${USERPROFILE}\.aspnet\https:/root/.aspnet/https/
