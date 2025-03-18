namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    internal abstract class Msg
    {
        public Header header { get; protected set; }
    }
}
