namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using UnityEngine;
    using Time = Primitives.Time;


    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/nav_msgs/html/msg/Path.html
    ///
    /// An array of poses that represents a Path for a robot to follow
    /// </summary>
    internal class PlanMsg : Msg
    {
        public Time timeStamp;
        public Pose[] poses { get; set; }

        public PlanMsg()
        {
            timeStamp = new Time();
            poses = new Pose[0];
        }

        public PlanMsg(Header header, Time timeStamp, Pose[] poses)
        {
            this.header = header;
            this.timeStamp = timeStamp;
            this.poses = poses;
        }
    }
}
