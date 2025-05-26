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
        writer.WritePropertyName("header");
        serializer.Serialize(writer, value.header);
        writer.WritePropertyName("info");
        serializer.Serialize(writer, value.info);
        writer.WritePropertyName("data");
        writer.WriteStartArray();
        foreach (var item in value.data)
        {
            writer.WriteValue(item);
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public override CostMapMsg ReadJson(JsonReader reader, Type objectType, CostMapMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        CostMapMsg costMapMsg = new CostMapMsg();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                return costMapMsg;
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                reader.Read();
                if(propertyName == "header")
                {
                    costMapMsg.header = serializer.Deserialize<Header>(reader);
                }
                else if (propertyName == "info")
                {
                    costMapMsg.info = serializer.Deserialize<CostMapMsg.Info>(reader);
                }
                else if (propertyName == "data")
                {
                    costMapMsg.data = serializer.Deserialize<int[]>(reader);
                }
                else
                {
                    Debug.LogError($"Unknown property: {propertyName}");
                }
            }
        }
        return costMapMsg;
    }
}
