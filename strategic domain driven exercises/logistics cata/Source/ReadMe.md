## Prerequisites

1. Install git - https://git-scm.com/downloads.
2. Clone this repository.
3. Install .NET 7.0 - https://dotnet.microsoft.com/download/dotnet/7.0.
4. Install Visual Studio, Rider or VSCode.
5. Install docker - https://docs.docker.com/engine/install/.
6. Open [Exercises.sln](./Exercises.sln) solution.

## Running

1. Run: `docker-compose up`.
2. Wait until all dockers got are downloaded and running.
3. You should automatically get:
    - Postgres DB running for [Marten storage](https://martendb.io)
    - PG Admin - IDE for postgres. Available at: http://localhost:5050.
        - Login: `admin@pgadmin.org`, Password: `admin`
        - To connect to server click right mouse on Servers, then Register Server and use host: `postgres`, user: `postgres`, password: `Password12!`
   - EventStoreDB UI: http://localhost:2113/
