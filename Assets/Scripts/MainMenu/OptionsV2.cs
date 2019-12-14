using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsV2 : MonoBehaviour
{
    //Initialize buttons/objects
    public Button gameButton;
    public Button controlButton;
    public Button videoButton;
    public Button audioButton;
    public Button backButton;

    public GameObject gamePanel;
    public GameObject controlPanel;
    public GameObject videoPanel;
    public GameObject audioPanel;

    void Start()
    {
        //Set OnClick listeners
        Button gamebtn = gameButton.GetComponent<Button>();
        gamebtn.onClick.AddListener(GameOnClick);
        Button ctrlbtn = controlButton.GetComponent<Button>();
        ctrlbtn.onClick.AddListener(ControlOnClick);
        Button vidbtn = videoButton.GetComponent<Button>();
        vidbtn.onClick.AddListener(VideoOnClick);
        Button audbtn = audioButton.GetComponent<Button>();
        audbtn.onClick.AddListener(AudioOnClick);
        Button backbtn = backButton.GetComponent<Button>();
        backbtn.onClick.AddListener(BackOnClick);

        //Set button positions
        Vector3 posCtrl = gamebtn.transform.position;
        posCtrl.y -= 30f;
        Vector3 posVid = gamebtn.transform.position;
        posVid.y -= 60f;
        Vector3 posAud = gamebtn.transform.position;
        posAud.y -= 90f;
        Vector3 posBack = gamebtn.transform.position;
        posBack.y -= 150f;

        //Grab button positions
        ctrlbtn.transform.position = posCtrl;
        vidbtn.transform.position = posVid;
        audbtn.transform.position = posAud;
        backbtn.transform.position = posBack;
    }

    //OnClick functions, switch panels
    void GameOnClick()
    {
        gamePanel.transform.gameObject.SetActive(true);
        controlPanel.transform.gameObject.SetActive(false);
        videoPanel.transform.gameObject.SetActive(false);
        audioPanel.transform.gameObject.SetActive(false);

        Debug.Log("Game Panel");
    }

    void ControlOnClick()
    {
        gamePanel.transform.gameObject.SetActive(false);
        controlPanel.transform.gameObject.SetActive(true);
        videoPanel.transform.gameObject.SetActive(false);
        audioPanel.transform.gameObject.SetActive(false);

        Debug.Log("Control Panel");
    }

    void VideoOnClick()
    {
        gamePanel.transform.gameObject.SetActive(false);
        controlPanel.transform.gameObject.SetActive(false);
        videoPanel.transform.gameObject.SetActive(true);
        audioPanel.transform.gameObject.SetActive(false);

        Debug.Log("Video Panel");
    }

    void AudioOnClick()
    {
        gamePanel.transform.gameObject.SetActive(false);
        controlPanel.transform.gameObject.SetActive(false);
        videoPanel.transform.gameObject.SetActive(false);
        audioPanel.transform.gameObject.SetActive(true);

        Debug.Log("Audio Panel");
    }

    void BackOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);

        Debug.Log("Back Button");
    }
}
