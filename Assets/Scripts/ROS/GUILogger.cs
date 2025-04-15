using UnityEngine;

public class GUILogger : MonoBehaviour
{
    public GameObject logText;


    // Start is called before the first frame update
    void Start()
    {
        logText.GetComponent<TMPro.TextMeshProUGUI>().text = "System's up!";

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Log(string message)
    {
        Debug.Log(message);
        logText.GetComponent<TMPro.TextMeshProUGUI>().text += "\n" + message;
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
        logText.GetComponent<TMPro.TextMeshProUGUI>().text += "\n" + message;
    }
}
