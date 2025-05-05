using Assets.Scripts.ROS.Network;
using UnityEngine;

public class ConnectionStatusUpdater : MonoBehaviour
{
    public WebSocketClient webSocketClient; // Ensure WebSocketClient is public or internal
    // Start is called before the first frame update
    void Start()
    {
        webSocketClient.AddOpenListener(() => OnOpen());
        webSocketClient.AddCloseListener((e) => OnClose());
    }

    // Update is called once per frame
    void Update()
    {

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
