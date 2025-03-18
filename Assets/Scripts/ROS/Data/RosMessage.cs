namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;

    internal class RosMessage
    {
        public string op { get; private set; }
        public string topic { get; private set; }
        public Msg msg { get; private set; }

        public RosMessage(string op, string topic, Msg msg)
        {
            this.op = op;
            this.topic = topic;
            this.msg = msg;
        }
    }
}
