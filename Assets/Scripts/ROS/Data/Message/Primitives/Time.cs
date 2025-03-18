namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using UnityEngine;

    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/std_msgs/html/msg/Time.html
    /// </summary>
    internal class Time : MonoBehaviour
    {
        public uint sec { get; private set; }
        public uint nanosec { get; private set; }
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
