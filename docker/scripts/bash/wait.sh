#!/bin/bash

echo Waiting $1 to be ready...

if hash apk 2>/dev/null
then
    apk update && apk add iputils
else
    apt-get update && apt-get install -y iputils-ping
fi

while ping -c1 $1
do 
    sleep 1 
done

echo $1 is ready
