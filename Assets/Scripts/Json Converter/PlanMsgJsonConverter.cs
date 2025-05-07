using Newtonsoft.Json;
using System;
using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;

internal class PlanMsgJsonConverter : JsonConverter<PlanMsg>
{
    public override void WriteJson(JsonWriter writer, PlanMsg value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("header");
        serializer.Serialize(writer, value.header);

        writer.WritePropertyName("poses");
        serializer.Serialize(writer, value.poses);

        writer.WriteEndObject();
    }

    public override PlanMsg ReadJson(JsonReader reader, Type objectType, PlanMsg existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
            throw new JsonSerializationException($"Expected StartObject token but got {reader.TokenType}");

        PlanMsg planMsg = existingValue ?? new PlanMsg();

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
                        planMsg.header = serializer.Deserialize<Header>(reader);
                        break;
                    case "poses":
                        planMsg.poses = serializer.Deserialize<PlanPose[]>(reader);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return planMsg;
    }
}
