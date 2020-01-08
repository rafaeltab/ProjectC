using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public static MusicLoop musicLoopInstance;
    public GameObject musicTextbox;
    private RectTransform rtTextbox;
    public GameObject musicText;
    private Vector2 oldSize;

    void Awake()
    {
        musicLoopInstance = MusicLoop.instance;
        rtTextbox = musicTextbox.GetComponent<RectTransform>();
        oldSize = rtTextbox.sizeDelta;
    }

    /// <summary>
    /// Changes audio by pressing right mouse button
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked && musicLoopInstance._as.clip.name != "BattleMusic")
        {
            if (musicLoopInstance.clipNumber == musicLoopInstance.audioClipArray.Length - 1)
            {
                musicLoopInstance.clipNumber = 0;
            }
            else
            {
                musicLoopInstance.clipNumber += 1;
            }

            musicLoopInstance.PlayMusic();
            UpdateText();
        }

        if (rtTextbox.sizeDelta != oldSize)
        {
            UpdatePos();
        }
    }

    /// <summary>
    /// Updates the music text
    /// </summary>
    public void UpdateText()
    {
        string[] strList = ("by " + musicLoopInstance._as.clip.name.Replace('_', ' ')).Split('-');
        musicText.GetComponent<TextMeshProUGUI>().text = strList[1] + "\n" + strList[0];
    }


    /// <summary>
    /// Updates the music textbox position
    /// </summary>
    public void UpdatePos()
    {
        float posX = musicText.GetComponent<RectTransform>().anchoredPosition.x;
        float posY = musicText.GetComponent<RectTransform>().anchoredPosition.y;

        rtTextbox.anchoredPosition = new Vector2(posX + 5, posY - 5);

        oldSize = rtTextbox.sizeDelta;
    }

    /// <summary>
    /// When equipping Radio
    /// </summary>
    public void OnEnable()
    {
        musicTextbox.SetActive(true);
        UpdateText();
    }

    public void OnDisable()
    {
        musicTextbox.SetActive(false);
    }
}
