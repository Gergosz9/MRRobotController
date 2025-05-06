namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using UnityEngine;
    using Time = Primitives.Time;

    /// <summary>
    /// CostMapMsg is a message type that contains information about a cost map,
    /// including its resolution, dimensions, and origin.
    /// </summary>
    internal class CostMapMsg : Msg
    {
        public class Info
        {
            public Time map_load_time { get; set; }
            public float resolution { get; set; }
            public uint width { get; set; }
            public uint height { get; set; }
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

        public Info info { get; private set; }
        public uint[] data { get; private set; }

        public CostMapMsg()
        {
            this.info = new Info();
            this.data = new uint[0];
        }

        public CostMapMsg(Header header, Info info, uint[] data)
        {
            this.header = header;
            this.info = info;
            this.data = data;
        }
    }
}
