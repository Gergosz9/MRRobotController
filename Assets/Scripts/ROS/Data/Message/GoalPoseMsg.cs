namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;

    internal class GoalPoseMsg : Msg
    {
        public Pose pose { get; private set; }
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
