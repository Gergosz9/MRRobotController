namespace Assets.Scripts.ROS.Network
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using UnityEngine;

    internal class ROSBridgeClient : MonoBehaviour
    {
        public GUILogger logger;
        [SerializeField]
        public WebSocketClient webSocketClient;

        public void Start()
        {
            webSocketClient.AddOpenListener(() => SubscribeToTopics());
        }

        public void StartCommunication()
        {
            webSocketClient.Connect();
        }

        private void SubscribeToTopics()
        {
            List<string> topics = new List<string>
            {
                Topic.scan,
                Topic.scansim,
                Topic.costmap,
                Topic.plan,
                Topic.plansmoothed,
                Topic.globalpath
            };

            topics.ForEach(topic => webSocketClient.Subscribe(topic));
            webSocketClient.AddMessageListener(HandleMessage);
        }
        private string GetMessageTopic(string jsonMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonMessage);
            return jsonObject["topic"].ToString();
        }

        int logcounter = 0;
        public void HandleMessage(string jsonMessage)
        {
            string topic = GetMessageTopic(jsonMessage);
            logger.Log(jsonMessage);
            //File.WriteAllText($"C:\\Users\\Eragon\\Documents\\Egyetem\\Onlab\\ROSlog2\\log{logcounter++}.json", jsonMessage);

            switch (topic)
            {
                case Topic.scan:
                case Topic.scansim:
                    RosMessage<ScanMsg> scanMessage = JsonConvert.DeserializeObject<RosMessage<ScanMsg>>(jsonMessage);
                    //handle scan message here
                    test(scanMessage);
                    break;
                case Topic.costmap:
                    RosMessage<CostMapMsg> costmapMessage = JsonConvert.DeserializeObject<RosMessage<CostMapMsg>>(jsonMessage);
                    //handle costmap message here
                    break;
                case Topic.plan:
                case Topic.plansmoothed:
                case Topic.globalpath:
                    RosMessage<PlanMsg> planMessage = JsonConvert.DeserializeObject<RosMessage<PlanMsg>>(jsonMessage);
                    //handle plan message here
                    break;
                default:
                    Debug.LogError($"Unknown topic: {topic}");
                    return;
            }
        }

        private void test(RosMessage<ScanMsg> scanMessage)
        {
            Vector3 prevousdot = new Vector3(0, 0, 0);
            for (int i = 0; i < scanMessage.msg.ranges.Length; i++)
            {
                float range = scanMessage.msg.ranges[i];
                float angle = scanMessage.msg.angle_min + scanMessage.msg.angle_increment * i;
                Vector3 dot = new Vector3(
                    range * Mathf.Cos(angle),
                    0,
                    range * Mathf.Sin(angle)
                );

                if (range >= .5f)
                {
                    if (Vector3.Distance(dot, prevousdot) < .5f)
                    {
                        Debug.DrawLine(prevousdot, dot, Color.green, .08f);
                    }
                    else if (Vector3.Distance(dot, prevousdot) < 1f)
                    {
                        Debug.DrawLine(prevousdot, dot, Color.yellow, .08f);
                    }
                    else
                    {
                        Debug.DrawLine(prevousdot, dot, Color.red, .08f);
                    }

                    prevousdot = dot;
                }
            }
            Debug.Log(scanMessage.msg.angle_min);
        }
    }
}
