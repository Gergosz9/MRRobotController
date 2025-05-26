using Newtonsoft.Json;
using System;
using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using UnityEngine;

internal class PlanMsgJsonConverter : JsonConverter<PlanMsg>
{
    public override void WriteJson(JsonWriter writer, PlanMsg value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("header");
        serializer.Serialize(writer, value.header);
        writer.WritePropertyName("poses");
        writer.WriteStartArray();
        foreach (var pose in value.poses)
        {
            serializer.Serialize(writer, pose);
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public override PlanMsg ReadJson(JsonReader reader, Type objectType, PlanMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        PlanMsg planMsg = new PlanMsg();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                return planMsg;
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                reader.Read();
                if (propertyName == "header")
                {
                    planMsg.header = serializer.Deserialize<Header>(reader);
                }
                else if (propertyName == "poses")
                {
                    var poses = new System.Collections.Generic.List<Pose>();
                    var poseConverter = new PoseJsonConverter();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                        Pose pose = Pose.identity;
                        pose = poseConverter.ReadJson(reader, typeof(Pose), pose, true, serializer);
                        poses.Add(pose);
                    }
                    planMsg.poses = poses.ToArray();
                }
                else
                {
                    Debug.LogError($"Unknown property: {propertyName}");
                }
            }
        }
        return planMsg;
    }
}
