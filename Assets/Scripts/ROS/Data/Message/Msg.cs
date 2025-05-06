namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.ROS.Data.Message.Primitives;

    /// <summary>
    /// Abstract class that serves as a base for all ROS message types.
    /// </summary>
    internal abstract class Msg
    {
        public Header header { get; set; }
    }
}
