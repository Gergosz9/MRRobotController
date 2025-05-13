namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using Assets.Scripts.Json_Converter.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;


    // <summary>
    // Represents a 3D transform consisting of translation and rotation.
    // </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(TransformJsonConverter))]
    internal class Transform
    {
        private Pose pose;

        [JsonProperty]
        public Vector3 translation
        {
            get { return pose.position; }
            set { pose.position = value; }
        }
        [JsonProperty]
        public Quaternion rotation
        {
            get { return pose.rotation; }
            set { pose.rotation = value; }
        }

        public Transform()
        {
            pose = new Pose();
        }

        public Transform(Vector3 translation, Quaternion rotation)
        {
            pose = new Pose(translation, rotation);
        }
    }
}
