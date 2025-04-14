namespace Assets.Scripts.ROS.Network
{
    using UnityEngine;
    using NativeWebSocket;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System;
    using System.Text;

    internal class WebSocketClient : MonoBehaviour
    {
        private static string serverAddress = "ws://192.168.100.81:9090";
        private static WebSocket webSocket = new WebSocket(serverAddress);

        public void Connect()
        {
            Debug.Log("[WebSocket] Connecting...");
            AddOpenListener(() => Debug.Log("[WebSocket] Connected"));
            AddMessageListener((message) => Debug.Log("[WebSocket] Message: " + message));
            AddErrorListener((e) => Debug.LogError("[WebSocket] Error: " + e));
            AddCloseListener((e) => Debug.Log("[WebSocket] Closed: " + e));

            webSocket.Connect();
        }

        private void OnDestroy()
        {
            webSocket.Close();
        }

        public void SendMessage(string message)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            webSocket.Send(encoded);
        }

        public void Subscribe(string topic)
        {
            string message = $"{{\"op\": \"subscribe\", \"topic\": \"{topic}\" }}";
            SendMessage(message);
            Debug.Log("Message sent: " + message);
        }

        public void AddMessageListener(System.Action<string> listener)
        {
            webSocket.OnMessage += (bytes) =>
            {
                listener(System.Text.Encoding.UTF8.GetString(bytes));
            };
        }

        public void AddOpenListener(System.Action listener)
        {
            webSocket.OnOpen += () =>
            {
                listener();
            };
        }

        public void AddCloseListener(System.Action<WebSocketCloseCode> listener)
        {
            webSocket.OnClose += (e) =>
            {
                listener(e);
            };
        }

        public void AddErrorListener(System.Action<string> listener)
        {
            webSocket.OnError += (e) =>
            {
                listener(e);
            };
        }

        internal void AddOpenListener(object v)
        {
            throw new NotImplementedException();
        }
    }

}
