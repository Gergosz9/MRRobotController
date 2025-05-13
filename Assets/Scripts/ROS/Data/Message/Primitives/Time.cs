namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using Assets.Scripts.Json_Converter.Message.Primitives;
    using Newtonsoft.Json;
    using UnityEngine;

    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/std_msgs/html/msg/Time.html
    ///
    /// Defines the Time message type used in ROS messages.
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(TimeJsonConverter))]
    internal class Time
    {
        [JsonProperty]
        public uint sec { get; set; }
        [JsonProperty]
        public uint nanosec { get; set; }
        public Time()
        {
            this.sec = 0;
            this.nanosec = 0;
        }
        public Time(uint sec, uint nanosec)
        {
            this.sec = sec;
            this.nanosec = nanosec;
        }
    }
}
