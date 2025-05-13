namespace Assets.Scripts.ROS.Data.Message
{
    using Assets.Scripts.Json_Converter.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(PathMsgJsonConverter))]
    internal class PathMsg:Msg
    {
        [JsonProperty]
        public PoseStamped[] poses;

        public PathMsg()
        {
            header = new Header();
            poses = new PoseStamped[0];
        }

        public PathMsg(Header header, PoseStamped[] poses)
        {
            this.header = header;
            this.poses = poses;
        }
    }
}
