namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    internal class ScanMsg : Msg
    {
        public float angle_min { get; set; }
        public float angle_max { get; set; }
        public float angle_increment { get; set; }
        public float time_increment { get; set; }
        public float scan_time { get; set; }
        public float range_min { get; set; }
        public float range_max { get; set; }
        public float?[] ranges { get; set; }
        public float?[] intensities { get; set; }
        public ScanMsg()
        {
            angle_min = 0.0f;
            angle_max = 0.0f;
            angle_increment = 0.0f;
            time_increment = 0.0f;
            scan_time = 0.0f;
            range_min = 0.0f;
            range_max = 0.0f;
            ranges = new float?[0];
            intensities = new float?[0];
        }
        public ScanMsg(Header header, float angle_min, float angle_max, float angle_increment, float time_increment, float scan_time, float range_min, float range_max, float?[] ranges, float?[] intensities)
        {
            this.header = header;
            this.angle_min = angle_min;
            this.angle_max = angle_max;
            this.angle_increment = angle_increment;
            this.time_increment = time_increment;
            this.scan_time = scan_time;
            this.range_min = range_min;
            this.range_max = range_max;
            this.ranges = ranges;
            this.intensities = intensities;
        }
    }
}
