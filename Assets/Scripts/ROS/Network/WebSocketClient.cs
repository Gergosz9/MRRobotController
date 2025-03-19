namespace Assets.Scripts.ROS.Network
{
    using UnityEngine;
    using WebSocketSharp;
    using Newtonsoft.Json;

    internal class WebSocketClient : MonoBehaviour
    {
        private WebSocket webSocket;

        void Start()
        {
            string serverAddress = "ws://cg.iit.bme.hu:16789/wsURL";
            webSocket = new WebSocket(serverAddress);

            webSocket.OnOpen += (sender, e) => Debug.Log("WebSocket opened");
            webSocket.OnMessage += (sender, e) => HandleMessage(e.Data);
            webSocket.OnError += (sender, e) => Debug.LogError("WebSocket error: " + e.Message);
            webSocket.OnClose += (sender, e) => Debug.Log("WebSocket closed with reason: " + e.Reason);

            webSocket.Connect();
        }

        void OnDestroy()
        {
            webSocket.Close();
        }

        private void HandleMessage(string message)
        {
            Debug.Log("Received message: " + message);
        }

        public void SendMessage(object message)
        {
            string jsonMessage = JsonConvert.SerializeObject(message);
            webSocket.Send(jsonMessage);
        }
    }

}
