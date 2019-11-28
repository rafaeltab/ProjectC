using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Start()
    {
        ListenSettings();
    }

    bool showFps = true;

    private void ListenSettings()
    {
        showFps = (bool) SettingsManager.GetInstance().Settings[3].GetSetting("showfps").Value;
        SettingsManager.GetInstance().Settings[3].GetSetting("showfps").changeEvent += (sender, value) =>
        {
            showFps = (bool) value;
        };
    }

    void Update()
    {
        
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (showFps) {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 10 / 100;
            style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}
