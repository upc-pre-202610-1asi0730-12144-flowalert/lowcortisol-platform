using Npgsql;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class PostgreSqlConnectionStringFactory
{
    private const string DefaultDatabaseName = "lowcortisol_db";
    private const string LegacyLocalDatabaseName = "lowcortisol_platform";

    public static string Create(IConfiguration configuration)
    {
        var databaseUrl = configuration["DATABASE_URL"];
        if (!string.IsNullOrWhiteSpace(databaseUrl))
            return NormalizeLocalDatabaseName(FromDatabaseUrl(databaseUrl));

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(connectionString) && !connectionString.Contains('%'))
            return NormalizeLocalDatabaseName(connectionString);

        var host = configuration["DATABASE_HOST"];
        var port = configuration["DATABASE_PORT"] ?? "5432";
        var database = configuration["DATABASE_NAME"] ?? DefaultDatabaseName;
        var username = configuration["DATABASE_USER"];
        var password = configuration["DATABASE_PASSWORD"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(database) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("PostgreSQL connection settings were not configured.");

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = int.Parse(port),
            Database = database,
            Username = username,
            Password = password,
            SslMode = SslMode.Require
        };

        return NormalizeLocalDatabaseName(builder.ConnectionString);
    }

    private static string FromDatabaseUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(0) ?? string.Empty),
            Password = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(1) ?? string.Empty),
            SslMode = SslMode.Require
        };

        return builder.ConnectionString;
    }

    private static string NormalizeLocalDatabaseName(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        if (IsLocalHost(builder.Host) &&
            (string.IsNullOrWhiteSpace(builder.Database) ||
             builder.Database.Equals(LegacyLocalDatabaseName, StringComparison.OrdinalIgnoreCase)))
        {
            builder.Database = DefaultDatabaseName;
        }

        return builder.ConnectionString;
    }

    private static bool IsLocalHost(string? host) =>
        host is "localhost" or "127.0.0.1" or "::1";
}
