namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [JsonObject(MemberSerialization.OptIn)]
    internal class TFMessage
    {
        [JsonProperty]
        public TransformStamped[] transforms { get; set; }

        public TFMessage()
        {
            transforms = new TransformStamped[0];
        }

        public TFMessage(TransformStamped[] transforms)
        {
            this.transforms = transforms;
        }
    }
}
