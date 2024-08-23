using System.Data;

namespace EventHub.Domain.Abstractions;

/// <summary>
/// Defines a factory for creating SQL database connections.
/// </summary>
/// <remarks>
/// Implementations of this interface should provide a way to create and configure instances of
/// database connections, typically for SQL databases. This abstraction allows for flexibility in
/// managing connections, including dependency injection and testing scenarios.
/// </remarks>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Creates and returns a new instance of a database connection.
    /// </summary>
    /// <returns>
    /// An <see cref="IDbConnection"/> instance that represents the connection to the SQL database.
    /// </returns>
    IDbConnection CreateConnection();
}
