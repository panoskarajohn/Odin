#!/bin/bash

DELAY=10

docker-compose --file replica_mongo.yml down
docker rm -f $(docker ps -a -q)
docker volume rm $(docker volume ls -q)

docker-compose --file replica_mongo.yml up -d

echo "****** Waiting for ${DELAY} seconds for containers to go up ******"
sleep $DELAY

docker exec mongo1 ./scripts/rs-init.sh