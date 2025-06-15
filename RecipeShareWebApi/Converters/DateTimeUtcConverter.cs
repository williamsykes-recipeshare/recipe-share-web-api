using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipeShareWebApi.Converters;

public class DateTimeUtcConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetDateTime(out var dateTime))
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(reader.GetDateTime(), DateTimeKind.Utc);

            return dateTime;

        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue) writer.WriteStringValue(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc));
        else writer.WriteNullValue();
    }
}