using Newtonsoft.Json;
using System;
using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using UnityEngine;

internal class ScanMsgJsonConverter : JsonConverter<ScanMsg>
{
    public override void WriteJson(JsonWriter writer, ScanMsg value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
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
        writer.WriteStartArray();
        foreach (var item in value.ranges)
        {
            writer.WriteValue(item);
        }
        writer.WriteEndArray();
        writer.WritePropertyName("intensities");
        writer.WriteStartArray();
        foreach (var item in value.intensities)
        {
            writer.WriteValue(item);
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public override ScanMsg ReadJson(JsonReader reader, Type objectType, ScanMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var scanMsg = new ScanMsg();

        if (reader.TokenType == JsonToken.StartObject)
        {
            reader.Read();
            while (reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value.ToString();
                    reader.Read();
                    if (propertyName == "header")
                    {
                        scanMsg.header = serializer.Deserialize<Header>(reader);
                    }
                    else if (propertyName == "angle_min")
                    {
                        scanMsg.angle_min = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "angle_max")
                    {
                        scanMsg.angle_max = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "angle_increment")
                    {
                        scanMsg.angle_increment = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "time_increment")
                    {
                        scanMsg.time_increment = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "scan_time")
                    {
                        scanMsg.scan_time = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "range_min")
                    {
                        scanMsg.range_min = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "range_max")
                    {
                        scanMsg.range_max = float.Parse(reader.Value.ToString());
                    }
                    else if (propertyName == "ranges")
                    {
                        scanMsg.ranges = serializer.Deserialize<float?[]>(reader);
                    }
                    else if (propertyName == "intensities")
                    {
                        scanMsg.intensities = serializer.Deserialize<float?[]>(reader);
                    }
                    else
                    {
                        Debug.LogError($"Unknown property: {propertyName}");
                    }
                    reader.Read();
                }
            }
        }
        return scanMsg;
    }
}
