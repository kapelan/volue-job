# Scope of task and general

Task is equivalent of what the Volue Platform team will be working on.
We are building cloud native software using containers on Kubernetes.
Since we are using timeseries data to build machine learning models to predict variables
in future, we need software that can scale, accept such data in a secure and scaleable manner 
and provide access to the  results.

Consider that this task is part of real solution, that will be deployed on production server.
It is expected we have multiple clients, data can be transferred in varying chunk sizes and 
processing time can vary, so parallelized calculations are needed. 

# Important info 

Task assumes that the canditate understand architecture ideas of cloud native applications and
microservices approach utilizing REST as a main endpoint for client. Internal communication 
between components is up to you.

It is expected that the candiate will finish task within 4 hours or less. We want working
solution made from good build blocks. If for any reason you feel there is no time to improve solution the way you want,
please provide additional #TODO comments

# Input & Output expected

Consider we are receving data from single customer. For simplicity you can assume only `example1` and `example2` data points will be used.

API service should accept such json
```json
[
    { "name": "example1", "t": 13515551, "v": 1.1 },
    { "name": "example1", "t": 13515552, "v": 2.4 },
    { "name": "example1", "t": 13515553, "v": 3.5 },
    { "name": "example2", "t": 13515554, "v": 1.5 },
    { "name": "example2", "t": 13515555, "v": 2.5 },
]
```

where:
- `name` : name of data point
- `t` : timestamp as epoch
- `v` : value

This endpoint should return 201 Accepted

API should provide query endpoint that optionally take in time range (eg. `from=13515551&to=13515553` means all data including `13515553` and `13515553` epoch) 
with calculation which will return

- Average of values in time range
- Sum of values in time range

**Example**

GET api/example1
```json
{
    "avg": 2.33, 
    "sum": 7
}
```

# Rules

1. Create two services: API and Calculation, preferably in different languages (C#, Python, Go)
2. Store data in any database of your choice
3. Calculations for API query must be done in the Calculation service
4. API service should have */health* endpoint that returns 200 OK if service is ready to accept data
5. Data points can have one or thousands of values
6. API service should have an Open API Specification endpoint
7. Each service should be run as a Docker container
8. There should be one bash/powershell script (or yaml pipeline) that will allow to:
- build
- setup
- run 
9. Containers will run on Linux
10. Code should be committed to a Git repository
