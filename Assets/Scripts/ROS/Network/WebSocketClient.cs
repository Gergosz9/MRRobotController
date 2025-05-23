﻿namespace Assets.Scripts.ROS.Network
{
    using NativeWebSocket;
    using System;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// WebSocketClient is a Unity MonoBehaviour that manages the connection to the Robot.
    /// Uses the NativeWebSocket library to handle WebSocket communication.
    /// </summary>
    public class WebSocketClient : MonoBehaviour
    {
        // default server address: "ws://192.168.100.81:9090"
        private static string serverAddress = "ws://192.168.100.81:9090";
        private static WebSocket webSocket = new WebSocket(serverAddress);

        private void Start()
        {
            AddOpenListener(() => Debug.Log("[WebSocket] Connected"));
            AddMessageListener((message) => Debug.Log("[WebSocket] Message: " + message));
            AddErrorListener((e) => Debug.Log("[WebSocket] Error: " + e));
            AddCloseListener((e) => Debug.Log("[WebSocket] Closed: " + e));
        }

        private void Update()
        {
            webSocket.DispatchMessageQueue();
        }

        public void Connect()
        {
            if (webSocket.State == WebSocketState.Connecting)
            {
                Debug.Log("[WebSocket] Connection already in progress...");
            }
            if (webSocket.State == WebSocketState.Closed)
            {
                Debug.Log("[WebSocket] Connecting...");
                webSocket.Connect();
            }
            if (webSocket.State == WebSocketState.Open)
            {
                Debug.Log("[WebSocket] Already connected.");
            }
        }

        private void OnDestroy()
        {
            webSocket.Close();
        }

        public new void SendMessage(string message)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            webSocket.Send(encoded);
            Debug.Log("[WebSocket] Sent: " + message);
        }

        public void Subscribe(string topic)
        {
            string message = $"{{\"op\": \"subscribe\", \"topic\": \"{topic}\" }}";
            SendMessage(message);
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
