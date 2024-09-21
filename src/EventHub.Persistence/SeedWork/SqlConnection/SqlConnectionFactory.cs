using System.Data;
using EventHub.Abstractions;
using Microsoft.Extensions.Configuration;

namespace EventHub.Persistence.SeedWork.SqlConnection;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnectionString") ?? "";
    }

    public IDbConnection CreateConnection()
    {
        var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}