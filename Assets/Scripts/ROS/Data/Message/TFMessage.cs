namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class TFMessage
    {
        [JsonProperty]
        public string op { get; set; }
        [JsonProperty]
        public string topic { get; set; }

        [JsonProperty]
        public TransformStamped[] transforms { get; set; }

        public TFMessage()
        {
            transforms = new TransformStamped[0];
        }

        public TFMessage(TransformStamped[] transforms)
        {
            this.transforms = transforms;
        }
    }
}
