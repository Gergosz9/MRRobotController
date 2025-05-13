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

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(PoseStampedJsonConverter))]
    internal class PoseStamped
    {
        [JsonProperty]
        public Header header { get; set; }
        [JsonProperty]
        public Pose pose { get; set; }
        public PoseStamped()
        {
            header = new Header();
            pose = new Pose();
        }
        public PoseStamped(Header header, Pose pose)
        {
            this.header = header;
            this.pose = pose;
        }
    }
}
