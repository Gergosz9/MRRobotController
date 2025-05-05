namespace Assets.Scripts.ROS.Network
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class ROSBridgeClient : MonoBehaviour
    {
        public GUILogger logger;
        [SerializeField]
        public WebSocketClient webSocketClient;
        [SerializeField]
        public LidarDisplay lidarDisplay;

        public void Start()
        {
            webSocketClient.AddOpenListener(() => SubscribeToTopics());
            StartCommunication();
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

            switch (topic)
            {
                case Topic.scan:
                case Topic.scansim:
                    RosMessage<ScanMsg> scanMessage = JsonConvert.DeserializeObject<RosMessage<ScanMsg>>(jsonMessage);
                    lidarDisplay.displayScan(scanMessage);
                    break;
                case Topic.costmap:
                    RosMessage<CostMapMsg> costmapMessage = JsonConvert.DeserializeObject<RosMessage<CostMapMsg>>(jsonMessage);
                    //handle costmap message here
                    break;
                case Topic.plan:
                case Topic.plansmoothed:
                case Topic.globalpath:
                    RosMessage<PlanMsg> planMessage = JsonConvert.DeserializeObject<RosMessage<PlanMsg>>(jsonMessage);
                    break;
                default:
                    Debug.LogError($"[ROSBridgeClient] Unknown topic: {topic}");
                    return;
            }
        }
    }
}
