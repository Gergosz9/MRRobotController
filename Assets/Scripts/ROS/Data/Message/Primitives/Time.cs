namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using UnityEngine;

    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/std_msgs/html/msg/Time.html
    ///
    /// Defines the Time message type used in ROS messages.
    /// </summary>
    internal class Time
    {
        public uint sec { get; set; }
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
