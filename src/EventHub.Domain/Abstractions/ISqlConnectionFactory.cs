using System.Data;

namespace EventHub.Domain.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}