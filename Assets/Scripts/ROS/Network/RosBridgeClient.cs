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
                    test(scanMessage);
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

                if(range >= .5f){
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
                //Debug.DrawLine(dot, dot + new Vector3(0, .3f, 0), Color.red, .2f);
            }
            Debug.Log(scanMessage.msg.angle_min);
        }
    }
}
