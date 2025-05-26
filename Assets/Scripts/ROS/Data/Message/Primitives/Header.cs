namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using Assets.Scripts.Json_Converter.Message.Primitives;
    using Newtonsoft.Json;
    using UnityEngine;


    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/std_msgs/html/msg/Header.html
    ///
    /// Header is a common message header used in ROS messages.
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(HeaderJsonConverter))]
    internal class Header
    {
        [JsonProperty]
        public Time stamp { get; set; }

        [JsonProperty]
        public string frame_id { get; set; }

        public Header()
        {
            this.stamp = new Time();
            this.frame_id = string.Empty;
        }
        public Header(Time stamp, string frame_id)
        {
            this.stamp = stamp;
            this.frame_id = frame_id;
        }
    }
}
