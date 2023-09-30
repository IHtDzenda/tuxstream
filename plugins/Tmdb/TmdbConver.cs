using Newtonsoft.Json;

public class DoubleConverter : JsonConverter<double>
{
    public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string stringValue = reader.Value.ToString();

        if (double.TryParse(stringValue, out double result))
        {
            return result;
        }

        return 0.0;
    }

    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}
public class IntConverter : JsonConverter<int>
{
    public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string stringValue = reader.Value.ToString();

        if (int.TryParse(stringValue, out int result))
        {
            return result;
        }
        return 0;
    }

    public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}
public class BoolConverter : JsonConverter<bool>
{
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string stringValue = reader.Value.ToString();
        if (bool.TryParse(stringValue, out bool result))
        {
            return result;
        }
        return false;
    }

    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}