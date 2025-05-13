namespace Assets.Scripts.Json_Converter.Message
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

    internal class GoalPoseMsgJsonConverter: JsonConverter<GoalPoseMsg>
    {
        public override void WriteJson(JsonWriter writer, GoalPoseMsg value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("header");
            serializer.Serialize(writer, value.header);
            writer.WritePropertyName("pose");
            serializer.Serialize(writer, value.pose);
            writer.WriteEndObject();
        }
        public override GoalPoseMsg ReadJson(JsonReader reader, Type objectType, GoalPoseMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            GoalPoseMsg goalPoseMsg = new GoalPoseMsg();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return goalPoseMsg;
                }
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value.ToString();
                    reader.Read();
                    if(propertyName == "header")
                    {
                        goalPoseMsg.header = serializer.Deserialize<Header>(reader);
                    }
                    else if (propertyName == "pose")
                    {
                        var poseConverter = new PoseJsonConverter();
                        goalPoseMsg.pose = poseConverter.ReadJson(reader, typeof(Pose), goalPoseMsg.pose, true, serializer);
                    }
                    else
                    {
                        Debug.Log($"Unknown property: {propertyName}");
                    }
                }
            }
            return goalPoseMsg;
        }
    }
}
