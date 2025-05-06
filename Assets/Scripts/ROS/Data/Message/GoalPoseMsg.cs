namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using UnityEngine;

    /// <summary>
    /// GoalPoseMsg is a message type that contains a pose (position and orientation) in 3D space.
    /// </summary>
    internal class GoalPoseMsg : Msg
    {
        public Pose pose { get; set; }
        public GoalPoseMsg()
        {
            pose = new Pose();
        }
        public GoalPoseMsg(Header header, Pose pose)
        {
            this.header = header;
            this.pose = pose;
        }
    }
}
