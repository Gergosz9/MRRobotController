namespace Assets.Scripts.Json_Converter.Message.Primitives
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using Time = ROS.Data.Message.Primitives.Time;

    internal class HeaderJsonConverter: JsonConverter<Header>
    {
        public override void WriteJson(JsonWriter writer, Header value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("stamp");
            serializer.Serialize(writer, value.stamp);
            writer.WritePropertyName("frame_id");
            writer.WriteValue(value.frame_id);
            writer.WriteEndObject();

        }
        public override Header ReadJson(JsonReader reader, Type objectType, Header existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var header = new Header();
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Value.ToString();
                        reader.Read();
                        if (propertyName == "stamp")
                        {
                            header.stamp = serializer.Deserialize<Time>(reader);
                        }
                        else if (propertyName == "frame_id")
                        {
                            header.frame_id = reader.Value.ToString();
                        }
                        else
                        {
                            Debug.LogError($"Unknown property: {propertyName}");
                        }
                    }
                    reader.Read();
                }
            }
            return header;
        }
    }
}
