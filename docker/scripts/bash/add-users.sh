#!/bin/bash

/tmp/docker/scripts/bash/wait.sh $1 &&

/opt/mssql-tools/bin/sqlcmd -S 10.6.0.11 -U sa -P SA_password123 -i /tmp/docker/sql/add-calculation-service-user.sql &&
/opt/mssql-tools/bin/sqlcmd -S 10.6.0.11 -U sa -P SA_password123 -i /tmp/docker/sql/add-web-api-user.sql
