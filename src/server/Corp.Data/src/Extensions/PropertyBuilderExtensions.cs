using System.Text.Json;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<Dictionary<string, string>?> HasDictionaryConversion(this PropertyBuilder<Dictionary<string, string>?> propertyBuilder, JsonSerializerOptions? options = null)
    {
        // Sqlite doesn't have JSON/B, so we'll use value converters to TEXT.
        return propertyBuilder.HasConversion(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, options));
    }

    public static PropertyBuilder<Dictionary<string, string>?> HasJsonbTags(this PropertyBuilder<Dictionary<string, string>?> propertyBuilder, bool isSqlite)
    {
        if (isSqlite)
        {
            return propertyBuilder.HasDictionaryConversion();
        }

        return propertyBuilder.HasColumnType("jsonb");
    }

    public static PropertyBuilder<string?> HasJsonb(this PropertyBuilder<string?> propertyBuilder, bool isSqlite)
    {
        if (isSqlite)
        {
            return propertyBuilder;
        }

        return propertyBuilder.HasColumnType("jsonb");
    }
}
