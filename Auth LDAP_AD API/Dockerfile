FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN adduser --disabled-password --home /app --gecos '' dotnetuser && chown -R dotnetuser /app

RUN ln -s /usr/lib/libldap-2.X.so.2.Y.Z /usr/lib/libldap.so.2 && ln -s /usr/lib/liblber-2.X.so.2.Y.Z /usr/lib/liblber.so.2

USER dotnetuser

WORKDIR /app

COPY ["../bin", "/app"]

EXPOSE 8080/tcp

ENV TZ=Europe/Paris
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "Auth LDAP_AD API.dll"]