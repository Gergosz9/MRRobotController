using Newtonsoft.Json;
using System;
using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using UnityEngine;
using Time = Assets.Scripts.ROS.Data.Message.Primitives.Time;

internal class CostMapMsgJsonConverter : JsonConverter<CostMapMsg>
{
    public override void WriteJson(JsonWriter writer, CostMapMsg value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        // Serialize header
        writer.WritePropertyName("header");
        serializer.Serialize(writer, value.header);

        // Serialize map metadata
        writer.WritePropertyName("info");
        writer.WriteStartObject();
        writer.WritePropertyName("map_load_time");
        serializer.Serialize(writer, value.info.map_load_time);
        writer.WritePropertyName("resolution");
        writer.WriteValue(value.info.resolution);
        writer.WritePropertyName("width");
        writer.WriteValue(value.info.width);
        writer.WritePropertyName("height");
        writer.WriteValue(value.info.height);
        writer.WritePropertyName("origin");
        serializer.Serialize(writer, value.info.origin);
        writer.WriteEndObject();

        // Serialize data array
        writer.WritePropertyName("data");
        serializer.Serialize(writer, value.data);

        writer.WriteEndObject();
    }

    public override CostMapMsg ReadJson(JsonReader reader, Type objectType, CostMapMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
            throw new JsonSerializationException($"Expected StartObject token but got {reader.TokenType}");

        var costMapMsg = new CostMapMsg();
        var info = new CostMapMsg.Info();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
                break;

            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = (string)reader.Value;
                reader.Read(); // Move to value

                switch (propertyName)
                {
                    case "header":
                        costMapMsg.header = serializer.Deserialize<Header>(reader);
                        break;
                    case "info":
                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                        {
                            if (reader.TokenType == JsonToken.PropertyName)
                            {
                                string subProperty = (string)reader.Value;
                                reader.Read();

                                switch (subProperty)
                                {
                                    case "map_load_time":
                                        info.map_load_time = serializer.Deserialize<Time>(reader);
                                        break;
                                    case "resolution":
                                        info.resolution = Convert.ToSingle(reader.Value);
                                        break;
                                    case "width":
                                        info.width = Convert.ToUInt32(reader.Value);
                                        break;
                                    case "height":
                                        info.height = Convert.ToUInt32(reader.Value);
                                        break;
                                    case "origin":
                                        info.origin = serializer.Deserialize<Pose>(reader);
                                        break;
                                    default:
                                        // Skip any properties that aren’t recognized.
                                        reader.Skip();
                                        break;
                                }
                            }
                        }
                        costMapMsg.info = info;
                        break;
                    case "data":
                        costMapMsg.data = serializer.Deserialize<int[]>(reader);
                        break;
                    default:
                        // Skip any properties that aren’t recognized.
                        reader.Skip();
                        break;
                }
            }
        }

        return costMapMsg;
    }
}
