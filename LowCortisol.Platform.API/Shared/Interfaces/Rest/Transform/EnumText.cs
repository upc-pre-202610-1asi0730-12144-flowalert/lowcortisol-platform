namespace LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

public static class EnumText
{
    public static string ToResourceValue<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var text = value.ToString();
        var result = new List<char>(text.Length + 4);

        for (var index = 0; index < text.Length; index++)
        {
            var current = text[index];
            if (char.IsUpper(current) && index > 0) result.Add('_');
            result.Add(char.ToLowerInvariant(current));
        }

        return new string(result.ToArray());
    }
}
