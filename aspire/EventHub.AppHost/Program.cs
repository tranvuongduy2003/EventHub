using EventHub.AppHost.Constants;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("eventhub-cache", 6379)
    .WithImageTag("7.0.15-alpine")
    .WithBindMount("cache_redis_data", "/data");

var mssqlSaPassword = builder.AddParameter("MSSqlSaPassword");
var mssql = builder.AddSqlServer("eventhub-db", mssqlSaPassword, 1437)
    .WithImageTag("2019-latest")
    .WithBindMount("sqlserver_data", "/var/lib/sqlserver/data");

var seq = builder.AddSeq("eventhub-seq", 8081)
    .WithImage("datalust/seq")
    .WithBindMount("seq_data", "/data")
    .WithEndpoint(5341, 5341, ProtocolScheme.Tcp, "connection");
var seqConnectionString = seq.Resource.GetEndpoint("connection");

var mongoUserName = builder.AddParameter("MongoUsername");
var mongoPassword = builder.AddParameter("MongoPassword");
var hangfire = builder.AddMongoDB("eventhub-hangfire", 27018)
    .WithImage("mongo")
    .WithEnvironment(context =>
    {
        context.EnvironmentVariables["MONGO_INITDB_ROOT_USERNAME"] = mongoUserName.Resource;
        context.EnvironmentVariables["MONGO_INITDB_ROOT_PASSWORD"] = mongoPassword.Resource;
    })
    .WithBindMount("hangfire_mongo_data", "/data/db")
    .AddDatabase("HangfireJobs");

// var enviroment = builder.AddParameter("AspNetCoreEnvironment");
// builder.AddProject<EventHub_Presentation>("eventhub-api")
//     .WithEnvironment("ASPNETCORE_ENVIRONMENT", enviroment)
//     .WithEnvironment("ConnectionStrings__CacheConnectionString", cache)
//     .WithEnvironment("ConnectionStrings__DefaultConnectionString",
//         $"Server=localhost,1437;Database=EventHubDB;User Id=sa;Password={mssqlSaPassword.Resource};TrustServerCertificate=True;Multipleactiveresultsets=true")
//     .WithEnvironment("SeqConfiguration__ServerUrl", seqConnectionString)
//     .WithEnvironment("HangfireSettings__Storage__ConnectionString", hangfire)
//     .WithExternalHttpEndpoints();

builder.Build().Run();