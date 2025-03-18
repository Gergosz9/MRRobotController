namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using UnityEngine;

    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/std_msgs/html/msg/Header.html
    /// </summary>
    internal class Header : MonoBehaviour
    {
        public Time stamp { get; private set; }
        public string frame_id { get; private set; }

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
