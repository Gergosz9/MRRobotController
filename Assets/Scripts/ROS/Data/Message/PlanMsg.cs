namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using UnityEngine;
    using Time = Primitives.Time;


    /// <summary>
    /// Naming convention: https://docs.ros.org/en/noetic/api/nav_msgs/html/msg/Path.html
    ///
    /// An array of poses that represents a Path for a robot to follow
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    //[JsonConverter(typeof(PlanMsgJsonConverter))]
    internal class PlanMsg : Msg
    {
        [JsonProperty]
        public Pose[] poses { get; set; }

        public PlanMsg()
        {
            Header header = new Header();
            poses = new Pose[0];
        }

        public PlanMsg(Header header, Pose[] poses)
        {
            this.header = header;
            this.poses = poses;
        }
    }
}
