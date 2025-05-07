namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract class that serves as a base for all ROS message types.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal abstract class Msg
    {
        [JsonProperty]
        public Header header { get; set; }
    }
}
