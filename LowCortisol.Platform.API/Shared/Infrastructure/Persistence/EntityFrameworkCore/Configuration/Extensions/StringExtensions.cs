using System.Text;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        var builder = new StringBuilder(value.Length + 8);

        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];

            if (char.IsUpper(character))
            {
                if (index > 0 && value[index - 1] != '_') builder.Append('_');
                builder.Append(char.ToLowerInvariant(character));
                continue;
            }

            builder.Append(character == '-' ? '_' : character);
        }

        return builder.ToString();
    }
}
