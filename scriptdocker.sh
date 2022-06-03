docker-compose down
docker rmi $(docker images -q)
dotnet publish --force --nologo ./Barrage\ API/Barrage\ API.csproj --configuration Release --framework net6.0 --output ./bin
docker build . -t barrageapi -f ./Barrage\ API/Dockerfile
rm -rf bin
dotnet publish --force --nologo ./Auth\ LDAP_AD\ API/Auth\ LDAP_AD\ API.csproj --configuration Release --framework net6.0 --output ./bin
docker build . -t authldapadapi -f ./Auth\ LDAP_AD\ API/Dockerfile
rm -rf bin
dotnet publish --force --nologo ./Service\ FTP/Service\ FTP.csproj --configuration Release --framework net6.0 --output ./bin
docker build . -t serviceftp -f ./Service\ FTP/Dockerfile
rm -rf bin
dotnet publish --force --nologo ./Service\ Température/Service\ Température.csproj --configuration Release --framework net6.0 --output ./bin
docker build . -t servicetemperature -f ./Service\ Température/Dockerfile
rm -rf bin
dotnet publish --force --nologo ./WebSite/WebSite.csproj --configuration Release --framework net6.0 --output ./bin
docker build . -t barragev3 -f ./WebSite/Dockerfile
rm -rf bin
