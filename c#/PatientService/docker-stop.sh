#!/bin/bash

ID=$(docker ps | grep Patient-service | awk '{printf $1}')

if [ ! -z "$ID" ]; then
	docker stop $ID
	docker rm $ID
else
	echo "Container not running."
fi
