using Newtonsoft.Json;
using System;
using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;

internal class ScanMsgJsonConverter : JsonConverter<ScanMsg>
{
    public override void WriteJson(JsonWriter writer, ScanMsg value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        // Serialize header using the default serializer
        writer.WritePropertyName("header");
        serializer.Serialize(writer, value.header);

        writer.WritePropertyName("angle_min");
        writer.WriteValue(value.angle_min);

        writer.WritePropertyName("angle_max");
        writer.WriteValue(value.angle_max);

        writer.WritePropertyName("angle_increment");
        writer.WriteValue(value.angle_increment);

        writer.WritePropertyName("time_increment");
        writer.WriteValue(value.time_increment);

        writer.WritePropertyName("scan_time");
        writer.WriteValue(value.scan_time);

        writer.WritePropertyName("range_min");
        writer.WriteValue(value.range_min);

        writer.WritePropertyName("range_max");
        writer.WriteValue(value.range_max);

        writer.WritePropertyName("ranges");
        serializer.Serialize(writer, value.ranges);

        writer.WritePropertyName("intensities");
        serializer.Serialize(writer, value.intensities);

        writer.WriteEndObject();
    }

    public override ScanMsg ReadJson(JsonReader reader, Type objectType, ScanMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
            throw new JsonSerializationException($"Expected StartObject token but got {reader.TokenType}");

        var scanMsg = new ScanMsg();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
                break;

            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = (string)reader.Value;
                reader.Read();

                switch (propertyName)
                {
                    case "header":
                        scanMsg.header = serializer.Deserialize<Header>(reader);
                        break;
                    case "angle_min":
                        scanMsg.angle_min = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "angle_max":
                        scanMsg.angle_max = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "angle_increment":
                        scanMsg.angle_increment = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "time_increment":
                        scanMsg.time_increment = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "scan_time":
                        scanMsg.scan_time = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "range_min":
                        scanMsg.range_min = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "range_max":
                        scanMsg.range_max = reader.Value != null ? Convert.ToSingle(reader.Value) : 0f;
                        break;
                    case "ranges":
                        scanMsg.ranges = serializer.Deserialize<float?[]>(reader);
                        break;
                    case "intensities":
                        scanMsg.intensities = serializer.Deserialize<float?[]>(reader);
                        break;
                    default:
                        // Skip any properties that aren’t recognized.
                        reader.Skip();
                        break;
                }
            }
        }

        return scanMsg;
    }
}
