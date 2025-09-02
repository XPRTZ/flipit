using Refit;
using System.Text.Json.Serialization;

namespace Xprtz.FlipIt.Contract;

public static class RefitSettingsFactory
{
    public static RefitSettings Create()
    {
        var serializer = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
        serializer.Converters.Remove(serializer.Converters.Single(x => x is JsonStringEnumConverter));

        return new() { ContentSerializer = new SystemTextJsonContentSerializer(serializer) };
    }
}
