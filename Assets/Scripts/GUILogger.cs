namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TMPro;
    using TMPro.Examples;
    using Unity.XR.CoreUtils;
    using UnityEngine;
    using UnityEngine.UI;

    internal class GUILogger : MonoBehaviour
    {
        private static GUILogger instance;
        private List<string> logMessages = new List<string>();
        [SerializeField]
        private GameObject textArea;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private int maxLogMessages = 50;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Application.logMessageReceived += HandleLog;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                Application.logMessageReceived -= HandleLog;
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if(logString.Length>128)
                logString = logString.Substring(0, 128) + "...";
            logMessages.Add(logString);

            if (logMessages.Count > maxLogMessages)
            {
                logMessages.RemoveAt(0);
            }

            if (textArea != null && textArea.GetComponent<TextMeshProUGUI>() != null)
            {
                textArea.GetComponent<TextMeshProUGUI>().text = string.Join("\n", logMessages);
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}
