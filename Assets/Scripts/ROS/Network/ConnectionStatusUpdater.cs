using Assets.Scripts.ROS.Network;
using UnityEngine;


/// <summary>
/// ConnectionStatusUpdater is a Unity MonoBehaviour that updates the connection status text based on the WebSocket connection state.
/// </summary>
public class ConnectionStatusUpdater : MonoBehaviour
{
    [SerializeField]
    private WebSocketClient webSocketClient;
    void Start()
    {
        webSocketClient.AddOpenListener(() => OnOpen());
        webSocketClient.AddCloseListener((e) => OnClose());
    }

    private void OnOpen()
    {
        gameObject.GetComponentInParent<TMPro.TextMeshProUGUI>().text = "Connected";
    }

    private void OnClose()
    {
        gameObject.GetComponentInParent<TMPro.TextMeshProUGUI>().text = "Disconnected";
    }
}
