using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public static Music musicInstance;
    public GameObject musicTextbox;
    private RectTransform rtTextbox;
    public GameObject musicText;
    private Vector2 oldSize;

    /// <summary>
    /// Get the music instance, RectTransform of the musicTextBox and the size of the musicTextBox
    /// </summary>
    void Awake()
    {
        musicInstance = Music.instance;
        rtTextbox = musicTextbox.GetComponent<RectTransform>();
        oldSize = rtTextbox.sizeDelta;
    }

    /// <summary>
    /// Changes audio by pressing right mouse button
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked && musicInstance._as.clip.name != "BattleMusic")
        {
            if (musicInstance.clipNumber == musicInstance.audioClipArray.Length - 1)
            {
                musicInstance.clipNumber = 0;
            }
            else
            {
                musicInstance.clipNumber += 1;
            }

            musicInstance.PlayMusic();
            UpdateText();
        }

        //Calls UpdatePos if textbox size changes
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
        string[] strList = ("by " + musicInstance._as.clip.name.Replace('_', ' ')).Split('-');
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

    /// <summary>
    /// When unequipping Radio
    /// </summary>
    public void OnDisable()
    {
        musicTextbox.SetActive(false);
    }
}
