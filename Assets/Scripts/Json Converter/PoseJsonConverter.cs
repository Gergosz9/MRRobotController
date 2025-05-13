using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseJsonConverter : JsonConverter<Pose>
{
    public override void WriteJson(JsonWriter writer, Pose value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("position");
        writer.WriteStartObject();
        writer.WritePropertyName("x"); writer.WriteValue(value.position.x);
        writer.WritePropertyName("y"); writer.WriteValue(value.position.y);
        writer.WritePropertyName("z"); writer.WriteValue(value.position.z);
        writer.WriteEndObject();

        writer.WritePropertyName("orientation");
        writer.WriteStartObject();
        writer.WritePropertyName("x"); writer.WriteValue(value.rotation.x);
        writer.WritePropertyName("y"); writer.WriteValue(value.rotation.y);
        writer.WritePropertyName("z"); writer.WriteValue(value.rotation.z);
        writer.WritePropertyName("w"); writer.WriteValue(value.rotation.w);
        writer.WriteEndObject();

        writer.WriteEndObject();
    }

    public override Pose ReadJson(JsonReader reader, Type objectType, Pose existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = (string)reader.Value;
                if (propertyName == "position")
                {
                    reader.Read(); // start object
                    float x = 0, y = 0, z = 0;
                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                    {
                        var name = (string)reader.Value;
                        reader.Read();
                        float val = Convert.ToSingle(reader.Value);
                        if (name == "x") x = val;
                        else if (name == "y") y = val;
                        else if (name == "z") z = val;
                    }
                    position = new Vector3(x, y, z);
                }
                else if (propertyName == "orientation")
                {
                    reader.Read(); // start object
                    float x = 0, y = 0, z = 0, w = 1;
                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                    {
                        var name = (string)reader.Value;
                        reader.Read();
                        float val = Convert.ToSingle(reader.Value);
                        if (name == "x") x = val;
                        else if (name == "y") y = val;
                        else if (name == "z") z = val;
                        else if (name == "w") w = val;
                    }
                    rotation = new Quaternion(x, y, z, w);
                }
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }

        return new Pose(position, rotation);
    }
}
