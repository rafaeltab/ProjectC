using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDimensionFlash : MonoBehaviour
{
    public Image whiteScreen;

    private KeyCode changeDimUp;
    private KeyCode changeDimDown;

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

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(changeDimUp) || Input.GetKeyDown(changeDimDown)) && !TutorialManager.cutsceneLock && Cursor.lockState == CursorLockMode.Locked)
        {
            StopAllCoroutines();
            StartCoroutine(ScreenFlash());
        }
    }


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
