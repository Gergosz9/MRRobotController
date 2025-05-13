namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [JsonObject(MemberSerialization.OptIn)]
    internal class TransformStamped
    {
        [JsonProperty]
        public Header header;

        [JsonProperty]
        public string child_frame_id;

        [JsonProperty]
        public Transform transform;

        public TransformStamped()
        {
            header = new Header();
            child_frame_id = string.Empty;
            transform = new Transform();
        }

        public TransformStamped(Header header, string child_frame_id, Transform transform)
        {
            this.header = header;
            this.child_frame_id = child_frame_id;
            this.transform = transform;
        }
    }
}
