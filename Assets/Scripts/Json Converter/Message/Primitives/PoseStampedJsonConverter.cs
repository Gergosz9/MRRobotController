namespace Assets.Scripts.Json_Converter.Message.Primitives
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class PoseStampedJsonConverter: JsonConverter<PoseStamped>
    {
        public override void WriteJson(JsonWriter writer, PoseStamped value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("header");
            serializer.Serialize(writer, value.header);
            writer.WritePropertyName("pose");
            serializer.Serialize(writer, value.pose);
            writer.WriteEndObject();
        }

        public override PoseStamped ReadJson(JsonReader reader, Type objectType, PoseStamped existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            PoseStamped poseStamped = new PoseStamped();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;
                    if (propertyName == "header")
                    {
                        poseStamped.header = serializer.Deserialize<Header>(reader);
                    }
                    else if (propertyName == "pose")
                    {
                        var poseConverter = new PoseJsonConverter();
                        poseStamped.pose = poseConverter.ReadJson(reader, typeof(Pose), poseStamped.pose, true, serializer);
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }
            return poseStamped;
        }
    }
}
