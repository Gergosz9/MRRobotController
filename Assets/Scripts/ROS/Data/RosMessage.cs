namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;

    /// <summary>
    /// RosMessage is a generic class that represents a message in the Robot Operating System (ROS) format.
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    internal class RosMessage<T> where T : Msg
    {
        [JsonProperty]
        public string op { get; set; }
        [JsonProperty]
        public string topic { get; set; }
        [JsonProperty]
        public T msg { get; set; }

        public RosMessage(string op, string topic, T msg)
        {
            this.op = op;
            this.topic = topic;
            this.msg = msg;
        }
    }
}
