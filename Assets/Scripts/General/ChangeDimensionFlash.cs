using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDimensionFlash : MonoBehaviour
{
    public Image whiteScreen;

    private KeyCode changeDimUp;
    private KeyCode changeDimDown;

    /// <summary>
    /// Gets the change dimension buttons from the settings
    /// </summary>
    private void ListenToSettings()
    {
        Setting wup = SettingsManager.GetInstance().Settings[0].GetSetting("wup");
        Setting wdown = SettingsManager.GetInstance().Settings[0].GetSetting("wdown");

        wup.changeEvent += (sender, value) =>
        {
            changeDimUp = (KeyCode)value;
        };

        changeDimUp = (KeyCode)wup.Value;

        wdown.changeEvent += (sender, value) =>
        {
            changeDimDown = (KeyCode)value;
        };

        changeDimDown = (KeyCode)wdown.Value;
    }
    // Start is called before the first frame update
    void Start()
    {
        ListenToSettings();
    }

    /// <summary>
    /// Flash when changing dimensions
    /// </summary>
    void Update()
    {
        if ((Input.GetKeyDown(changeDimUp) || Input.GetKeyDown(changeDimDown)) && !TutorialManager.cutsceneLock && Cursor.lockState == CursorLockMode.Locked)
        {
            StopAllCoroutines();
            StartCoroutine(ScreenFlash());
        }
    }

    /// <summary>
    /// White screen flash
    /// </summary>
    /// <returns></returns>
    IEnumerator ScreenFlash()
    {
        byte opacity = 255;
        while (true)
        {
            opacity -= 2;
            
            whiteScreen.color = new Color32(255, 255, 255, opacity);
            yield return new WaitForSecondsRealtime(0.01f);

            if (opacity < 5)
            {
                whiteScreen.color = new Color32(255, 255, 255, 0);
                break;
            }
        }
    }
}
