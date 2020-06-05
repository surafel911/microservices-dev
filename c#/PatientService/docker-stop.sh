#!/bin/bash

ID=$(docker ps -a | grep patient-service | awk '{printf $1}')

if [ ! -z "$ID" ]; then
	docker stop $ID
	docker rm $ID
else
	echo "Container not running."
fi
