#!/bin/bash

DELAY=1
# Careful with the path, we call it from the start_infra.sh so it uses root path
docker-compose --file ./Mongo/replica_mongo.yml up -d

echo "****** Waiting for ${DELAY} seconds for containers to go up ******"
sleep $DELAY

docker exec mongo1 "//scripts//rs-init.sh"

