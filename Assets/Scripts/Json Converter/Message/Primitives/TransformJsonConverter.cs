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
    using UnityEngine.UIElements;
    using Transform = ROS.Data.Message.Primitives.Transform;

    internal class TransformJsonConverter: JsonConverter<Transform>
    {
        public override void WriteJson(JsonWriter writer, Transform value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("translation");
            serializer.Serialize(writer, value.translation);
            writer.WritePropertyName("rotation");
            serializer.Serialize(writer, value.rotation);
            writer.WriteEndObject();
        }
        public override Transform ReadJson(JsonReader reader, Type objectType, Transform existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Vector3 translation = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;
                    if (propertyName == "translation")
                    {
                        reader.Read();
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
                        translation = new Vector3(x, y, z);
                    }
                    else if (propertyName == "rotation")
                    {
                        reader.Read();
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
                    else
                    {
                        Debug.LogError($"Unknown property: {propertyName}");
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return new Transform(translation, rotation);
        }
    }
}
