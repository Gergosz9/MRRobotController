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
        public WebSocketClient webSocketClient { get; private set; }

        private void Start()
        {
            List<string> topics = new List<string>();
            topics.Add(Topic.scan);
            topics.Add(Topic.scansim);
            topics.Add(Topic.costmap);
            topics.Add(Topic.plan);
            topics.Add(Topic.plansmoothed);

            SubscribeToTopics(topics);
        }

        private void SubscribeToTopic(string topic)
        {
            webSocketClient.SendMessage($"{{\"op\": \"subscribe\", \"topic\": \"{topic}\"}}");
        }
        private void SubscribeToTopics(List<string> topics)
        {
            topics.ForEach(topic => SubscribeToTopic(topic));
        }
        private string GetMessageTopic(string jsonMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonMessage);
            return jsonObject["topic"].ToString();
        }

        public void HandleMessage(string jsonMessage)
        {
            string topic = GetMessageTopic(jsonMessage);

            Msg msg;
            switch (topic)
            {
                case Topic.scan:
                    {
                        msg = JsonConvert.DeserializeObject<ScanMsg>(jsonMessage);
                        //Handle message
                        break;
                    }
                case Topic.scansim:
                    {
                        msg = JsonConvert.DeserializeObject<ScanMsg>(jsonMessage);
                        //Handle message
                        break;
                    }
                case Topic.costmap:
                    {
                        msg = JsonConvert.DeserializeObject<CostMapMsg>(jsonMessage);
                        //Handle message
                        break;
                    }
                case Topic.plan:
                    {
                        msg = JsonConvert.DeserializeObject<PlanMsg>(jsonMessage);
                        //Handle message
                        break;
                    }
                case Topic.plansmoothed:
                    {
                        msg = JsonConvert.DeserializeObject<PlanMsg>(jsonMessage);
                        //Handle message
                        break;
                    }
                default:
                    {
                        Debug.LogError("Unknown topic: " + topic);
                        return;
                    }
            }

            double milisecondsnow = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
            double milisecondsmessage = TimeSpan.FromTicks(msg.header.stamp.sec + msg.header.stamp.nanosec / 1000000).TotalMilliseconds;

            Debug.Log($"Handled message: [{msg.header.frame_id}] after {milisecondsnow - milisecondsmessage} ms");
        }
        public void PublishMessage(Msg msg)
        {
            webSocketClient.SendMessage(msg);
        }

        public void SetWebSocketClient(WebSocketClient webSocketClient)
        {
            try
            {
                this.webSocketClient = webSocketClient;
            }
            catch
            {
                Debug.LogError("Failed to set WebSocketClient");
            }
        }
    }
}
