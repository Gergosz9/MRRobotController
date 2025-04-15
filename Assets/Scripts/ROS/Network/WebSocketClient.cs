namespace Assets.Scripts.ROS.Network
{
    using NativeWebSocket;
    using System;
    using System.Text;
    using UnityEngine;

    public class WebSocketClient : MonoBehaviour
    {
        public static string serverAddress = "ws://192.168.100.81:9090";
        private static WebSocket webSocket = new WebSocket(serverAddress);
        public GUILogger logger;

        public void Connect()
        {
            logger.Log("[WebSocket] Connecting...");
            AddOpenListener(() => logger.Log("[WebSocket] Connected"));
            AddMessageListener((message) => logger.Log("[WebSocket] Message: " + message));
            AddErrorListener((e) => logger.LogError("[WebSocket] Error: " + e));
            AddCloseListener((e) => logger.Log("[WebSocket] Closed: " + e));

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
            logger.Log("Message sent: " + message);
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
