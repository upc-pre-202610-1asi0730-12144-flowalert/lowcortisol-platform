using Npgsql;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class PostgreSqlDatabaseInitializer
{
    public static async Task EnsureDatabaseCreatedAsync(
        string connectionString,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        if (!IsLocalHost(builder.Host) || string.IsNullOrWhiteSpace(builder.Database))
            return;

        var databaseName = builder.Database;
        builder.Database = "postgres";

        await using var connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var existsCommand = connection.CreateCommand();
        existsCommand.CommandText = "SELECT 1 FROM pg_database WHERE datname = @databaseName";
        existsCommand.Parameters.AddWithValue("databaseName", databaseName);

        var exists = await existsCommand.ExecuteScalarAsync(cancellationToken);
        if (exists is not null) return;

        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = $"CREATE DATABASE {QuoteIdentifier(databaseName)}";
        await createCommand.ExecuteNonQueryAsync(cancellationToken);

        logger.LogInformation("Created local PostgreSQL database {DatabaseName}.", databaseName);
    }

    private static bool IsLocalHost(string? host) =>
        host is "localhost" or "127.0.0.1" or "::1";

    private static string QuoteIdentifier(string identifier) =>
        "\"" + identifier.Replace("\"", "\"\"") + "\"";
}
