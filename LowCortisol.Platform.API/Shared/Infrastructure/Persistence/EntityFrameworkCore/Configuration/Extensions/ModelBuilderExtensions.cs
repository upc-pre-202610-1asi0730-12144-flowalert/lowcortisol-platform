using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrWhiteSpace(tableName)) entity.SetTableName(tableName.ToSnakeCase());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrWhiteSpace(keyName)) key.SetName(keyName.ToSnakeCase());
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var constraintName = foreignKey.GetConstraintName();
                if (!string.IsNullOrWhiteSpace(constraintName))
                {
                    foreignKey.SetConstraintName(constraintName.ToSnakeCase());
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                var databaseName = index.GetDatabaseName();
                if (!string.IsNullOrWhiteSpace(databaseName)) index.SetDatabaseName(databaseName.ToSnakeCase());
            }
        }
    }
}
