#!/bin/bash

# clean all containers and volumes
#docker stop $(docker ps -a -q)
#docker rm -f $(docker ps -a -q)
#docker volume rm $(docker volume ls -q)

docker-compose --file ../infra.yml up -d

exec ./Mongo/start_replica.sh