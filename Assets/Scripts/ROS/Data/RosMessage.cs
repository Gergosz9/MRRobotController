namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;

    internal class RosMessage<T> where T : Msg
    {
        public string op { get; set; }
        public string topic { get; set; }
        public T msg { get; set; }

        public RosMessage(string op, string topic, T msg)
        {
            this.op = op;
            this.topic = topic;
            this.msg = msg;
        }
    }
}
