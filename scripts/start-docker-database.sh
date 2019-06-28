#!/bin/bash
docker run -p 5433:5432 --name pg-locationservice -e POSTGRES_USER=integrator -e POSTGRES_PASSWORD=inteword -e POSTGRES_DB=locationservice -d postgres:11.4
