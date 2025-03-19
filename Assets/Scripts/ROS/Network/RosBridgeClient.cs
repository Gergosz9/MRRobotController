namespace Assets.Scripts.ROS.Network
{
    using UnityEngine;
    using Newtonsoft.Json;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using System.Collections.Generic;
    using Assets.Scripts.ROS.Data.Message;
    using System;

    internal class ROSBridgeClient : MonoBehaviour
    {
        [SerializeField]
        public WebSocketClient webSocketClient;


        private void Start()
        {
            webSocketClient.Connect();
            SubscribeToTopics();
        }

        private void SubscribeToTopics()
        {
            List<string> topics = new List<string>();
            topics.Add(Topic.scan);
            topics.Add(Topic.scansim);
            topics.Add(Topic.costmap);
            topics.Add(Topic.plan);
            topics.Add(Topic.plansmoothed);

            topics.ForEach(topic => webSocketClient.Subscribe(topic));
            webSocketClient.AddMessageListener(HandleMessage);
        }
        private string GetMessageTopic(string jsonMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonMessage);
            return jsonObject["topic"].ToString();
        }

        public void HandleMessage(string jsonMessage)
        {
            string topic = GetMessageTopic(jsonMessage);

            switch (topic)
            {
                case Topic.scan:
                case Topic.scansim:
                    RosMessage<ScanMsg> scanMessage = JsonConvert.DeserializeObject<RosMessage<ScanMsg>>(jsonMessage);
                    break;
                case Topic.costmap:
                    RosMessage<CostMapMsg> costmapMessage = JsonConvert.DeserializeObject<RosMessage<CostMapMsg>>(jsonMessage);
                    break;
                case Topic.plan:
                case Topic.plansmoothed:
                    RosMessage<PlanMsg> planMessage = JsonConvert.DeserializeObject<RosMessage<PlanMsg>>(jsonMessage);
                    break;
                default:
                    Debug.LogError($"Unknown topic: {topic}");
                    return;
            }
        }
    }
}
