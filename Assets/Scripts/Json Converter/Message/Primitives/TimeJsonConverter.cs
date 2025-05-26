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

    internal class TimeJsonConverter: JsonConverter<Time>
    {
        public override void WriteJson(JsonWriter writer, Time value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("sec");
            writer.WriteValue(value.sec);
            writer.WritePropertyName("nanosec");
            writer.WriteValue(value.nanosec);
            writer.WriteEndObject();
        }
        public override Time ReadJson(JsonReader reader, Type objectType, Time existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var time = new Time();
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Value.ToString();
                        reader.Read();
                        if (propertyName == "sec")
                        {
                            time.sec = Convert.ToUInt32(reader.Value);
                        }
                        else if (propertyName == "nanosec" || propertyName == "nsec")
                        {
                            time.nanosec = Convert.ToUInt32(reader.Value);
                        }
                        else
                        {
                            Debug.LogError($"Unknown property: {propertyName}");
                        }
                    }
                    reader.Read();
                }
            }
            return time;
        }
    }
}
