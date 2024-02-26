#!/bin/bash

docker compose up -d

echo "Waiting 5 seconds for the database deployment..."
sleep 5

docker exec reports-db /scripts/rs-init.sh