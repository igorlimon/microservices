FROM microsoft/dotnet:2.2.0-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:5051
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Course.Identity.dll
