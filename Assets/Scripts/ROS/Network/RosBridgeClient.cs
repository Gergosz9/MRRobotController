namespace Assets.Scripts.ROS.Network
{
    using Assets.Scripts.ROS.Data.Message;
    using Assets.Scripts.ROS.Data.Message.Primitives;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Handles the messages received from the websocket.
    /// </summary>
    internal class ROSBridgeClient : MonoBehaviour
    {
        [SerializeField]
        private WebSocketClient webSocketClient;
        [SerializeField]
        private LidarDisplay lidarDisplay;

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
            using (var stringReader = new StringReader(jsonMessage))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName &&
                        (string)jsonReader.Value == "topic")
                    {
                        jsonReader.Read();
                        return jsonReader.Value?.ToString();
                    }
                }
            }

            Debug.Log("Topic field not found in JSON message.");
            return null;
        }

        public void HandleMessage(string jsonMessage)
        {
            string topic = GetMessageTopic(jsonMessage);

            switch (topic)
            {
                case Topic.scan:
                case Topic.scansim:
                    RosMessage<ScanMsg> scanMessage = JsonConvert.DeserializeObject<RosMessage<ScanMsg>>(jsonMessage);
                    lidarDisplay.UpdatePoints(scanMessage);
                    break;
                case Topic.costmap:
                    RosMessage<CostMapMsg> costmapMessage = JsonConvert.DeserializeObject<RosMessage<CostMapMsg>>(jsonMessage);
                    //handle costmap message here
                    break;
                case Topic.plan:
                case Topic.plansmoothed:
                case Topic.globalpath:
                    //RosMessage<PlanMsg> planMessage = JsonConvert.DeserializeObject<RosMessage<PlanMsg>>(jsonMessage);
                    //handle plan message here
                    break;
                default:
                    Debug.Log($"[ROSBridgeClient] Unknown topic: {topic}");
                    return;
            }
        }
    }
}
