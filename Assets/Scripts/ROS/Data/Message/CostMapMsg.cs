namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using UnityEngine;
    using Time = Primitives.Time;

    /// <summary>
    /// CostMapMsg is a message type that contains information about a cost map,
    /// including its resolution, dimensions, and origin.
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(CostMapMsgJsonConverter))]
    internal class CostMapMsg : Msg
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Info
        {
            [JsonProperty]
            public Time map_load_time { get; set; }
            [JsonProperty]
            public float resolution { get; set; }
            [JsonProperty]
            public uint width { get; set; }
            [JsonProperty]
            public uint height { get; set; }
            [JsonProperty]
            [JsonConverter(typeof(PoseJsonConverter))]
            public Pose origin { get; set; }

            public Info()
            {
                this.map_load_time = new Time();
                this.resolution = 0.0f;
                this.width = 0;
                this.height = 0;
                this.origin = new Pose();
            }
            public Info(Time map_load_time, float resolution, uint width, uint height, Pose origin)
            {
                this.map_load_time = map_load_time;
                this.resolution = resolution;
                this.width = width;
                this.height = height;
                this.origin = origin;
            }
        }
        [JsonProperty]
        public Info info { get; set; }
        [JsonProperty]
        public int[] data { get; set; }

        public CostMapMsg()
        {
            this.info = new Info();
            this.data = new int[0];
        }

        public CostMapMsg(Header header, Info info, int[] data)
        {
            this.header = header;
            this.info = info;
            this.data = data;
        }
    }
}
