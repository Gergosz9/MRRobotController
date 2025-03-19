namespace Assets.Scripts.ROS.Network
{
    using UnityEngine;
    using WebSocketSharp;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    internal class WebSocketClient : MonoBehaviour
    {
        private static string serverAddress = "ws://localhost:8765";
        private static WebSocket webSocket = new WebSocket(serverAddress);

        public void Connect()
        {
            webSocket.OnOpen += (sender, e) => Debug.Log("WebSocket opened");
            webSocket.OnMessage += (sender, e) => HandleMessage(e.Data);
            webSocket.OnError += (sender, e) => Debug.LogError("WebSocket error: " + e.Message);
            webSocket.OnClose += (sender, e) => Debug.Log("WebSocket closed with reason: " + e.Reason);

            webSocket.Connect();
        }

        private void OnDestroy()
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

        public void Subscribe(string topic)
        {
            string message = $"{{\"op\": \"subscribe\", \"topic\": \"{topic}\"}}";
            SendMessage(message);
        }

        public void AddMessageListener(System.Action<string> listener)
        {
            webSocket.OnMessage += (sender, e) => listener(e.Data);
        }

        public void AddOpenListener(System.Action listener)
        {
            webSocket.OnOpen += (sender, e) => listener?.Invoke();
        }
    }

}
