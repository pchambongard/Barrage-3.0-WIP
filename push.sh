
docker image tag barragev3 images-docker.intra.cg30.fr/pch/barragev3
docker image tag authldapadapi images-docker.intra.cg30.fr/pch/authldapadapi
docker image tag barrageapi images-docker.intra.cg30.fr/pch/barrageapi
docker image tag serviceftp images-docker.intra.cg30.fr/pch/serviceftp
docker image tag servicetemperature images-docker.intra.cg30.fr/pch/servicetemperature

docker push images-docker.intra.cg30.fr/pch/barragev3
docker push images-docker.intra.cg30.fr/pch/authldapadapi
docker push images-docker.intra.cg30.fr/pch/barrageapi
docker push images-docker.intra.cg30.fr/pch/serviceftp
docker push images-docker.intra.cg30.fr/pch/servicetemperature