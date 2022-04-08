FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN adduser --disabled-password --home /app --gecos '' dotnetuser && chown -R dotnetuser /app

USER dotnetuser

WORKDIR /app

COPY ["../bin", "/app"]

EXPOSE 8080/tcp

ENV TZ=Europe/Paris
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "WebSite.dll"]
