namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    [JsonObject(MemberSerialization.OptIn)]
    internal class PlanPose
    {
        [JsonProperty]
        Header header { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(PoseJsonConverter))]
        Pose pose { get; set; }

        public PlanPose()
        {
            this.header = new Header();
            this.pose = new Pose();
        }

        public PlanPose(Header header, Pose pose)
        {
            this.header = header;
            this.pose = pose;
        }
    }
}
