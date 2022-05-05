# Development

## Add Migration(s)

add migrations(s) by `dotnet ef`

```
dotnet ef migrations add MigrationName -p Infrastructure  -s ChatHub.API -c ChatAppContext -o Persistence\Migrations
```

or add migrations(s) in Visual Studio

```
PM> Add-Migration Migration(s)Name -Project Infrastructure -StartupProject ChatHub.API -Context ChatAppContext -OutputDir "Persistence\Migrations"
```
## ChatApp rely on redis and rabbitmq

run *redis* using docker

```
docker run --name myredis -d redis redis-server --save 60 1 --loglevel warning -p 6379:6379
```
run  *rabbitmq* using docker

```
docker run -d -p 15672:15672 -p 5672:5672 -p 5671:5671 --hostname my-rabbitmq --name my-rabbitmq-container rabbitmq:3-management
```

