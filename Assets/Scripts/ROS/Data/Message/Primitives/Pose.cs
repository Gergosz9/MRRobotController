namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    using UnityEngine;

    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/geometry_msgs/html/msg/Pose.html
    /// </summary>
    internal class Pose
    {
        public Vector3 position { get; set; }
        public Quaternion orientation { get; set; }

        public Pose()
        {
            this.position = Vector3.zero;
            this.orientation = Quaternion.identity;
        }
        public Pose(Vector3 position, Quaternion orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }
    }

}
