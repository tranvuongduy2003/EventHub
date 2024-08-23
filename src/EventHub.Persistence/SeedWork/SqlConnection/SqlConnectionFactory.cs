using System.Data;
using EventHub.Domain.Abstractions;
using Microsoft.Extensions.Configuration;

namespace EventHub.Persistence.SeedWork.SqlConnection;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string? _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnectionString");
    }

    public IDbConnection CreateConnection()
    {
        return new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
    }
}