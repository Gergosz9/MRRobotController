namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.Json_Converter.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using UnityEngine;

    /// <summary>
    /// GoalPoseMsg is a message type that contains a pose (position and orientation) in 3D space.
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(GoalPoseMsgJsonConverter))]
    internal class GoalPoseMsg : Msg
    {
        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(PoseJsonConverter))]
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
