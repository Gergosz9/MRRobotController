namespace Assets.Scripts.Json_Converter.Message
{
    using Assets.Scripts.Json_Converter.Message.Primitives;
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class PathMsgJsonConverter:JsonConverter<PathMsg>
    {
        public override void WriteJson(JsonWriter writer, PathMsg value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("header");
            serializer.Serialize(writer, value.header);
            writer.WritePropertyName("poses");
            writer.WriteStartArray();
            foreach (var pose in value.poses)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("pose");
                serializer.Serialize(writer, pose.pose);
                writer.WritePropertyName("header");
                serializer.Serialize(writer, pose.header);
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
        public override PathMsg ReadJson(JsonReader reader, Type objectType, PathMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            PathMsg pathMsg = new PathMsg();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return pathMsg;
                }
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value.ToString();
                    reader.Read();
                    if (propertyName == "header")
                    {
                        pathMsg.header = serializer.Deserialize<Header>(reader);
                    }
                    else if (propertyName == "poses")
                    {
                        var poses = new List<PoseStamped>();
                        var poseStampedConverter = new PoseStampedJsonConverter();
                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                        {
                            PoseStamped poseStamped = new PoseStamped();
                            poseStamped = poseStampedConverter.ReadJson(reader, typeof(PoseStamped), poseStamped, true, serializer);
                            poses.Add(poseStamped);
                        }
                        pathMsg.poses = poses.ToArray();
                    }
                    else
                    {
                        Debug.Log($"Unknown property: {propertyName}");
                    }
                }
            }
            return pathMsg;
        }
    }
}
