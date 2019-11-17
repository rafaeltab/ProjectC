using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]

public class OptionsMenu : MonoBehaviour
{
    public List<OptionsPage> OptionsPages { get; set; } = new List<OptionsPage>();

    public Button buttonTemplate;
    public Vector2 sizePercentage = new Vector2(0.75f, 0.75f);

    public void Start()
    {
        foreach(var settingsPage in SettingsManager.GetInstance().Settings)
        {
            OptionsPage op = new OptionsPage(buttonTemplate, settingsPage, GetComponent<Canvas>(), this, sizePercentage);
            OptionsPages.Add(op);
        }
    }

    public void Enable(OptionsPage optionsPage)
    {
        foreach(var op in OptionsPages)
        {
            if(op != optionsPage)
            {
                op.Disable();
            }
        }
    }
}

public class OptionsPage
{
    public Button pageButton;
    Settings settings;
    Canvas parentCanvas;
    OptionsMenu optionsMenu;
    Vector2 sizePercentage;

    public OptionsPage(Button pageButton, Settings settings, Canvas parentCanvas, OptionsMenu optionsMenu, Vector2 sizePercentage)
    {
        this.pageButton = GameObject.Instantiate(pageButton, parentCanvas.transform);
        
        this.settings = settings;
        this.parentCanvas = parentCanvas;
        this.optionsMenu = optionsMenu;
        this.sizePercentage = sizePercentage;
        
        HandleButton();
    }

    public void HandleClick()
    {
        Debug.Log(settings.ClassName);
        optionsMenu.Enable(this);        
    }

    public void Disable()
    {
        
    }

    public void HandleButton()
    {
        Vector3 pos = new Vector3(0, Screen.height * (sizePercentage.y), 0);
        if (optionsMenu.OptionsPages.Count > 0)
        {
            OptionsPage last = optionsMenu.OptionsPages[optionsMenu.OptionsPages.Count - 1];
            pos.x = last.pageButton.transform.position.x + last.pageButton.GetComponent<RectTransform>().rect.width;
        }
        else
        {
            pos.x = ((1-sizePercentage.x)/2)*Screen.width;
        }

        pageButton.transform.position = pos;
        pageButton.onClick.AddListener(()=> { Debug.Log("Click"); });
        pageButton.onClick.Invoke();
        Debug.Log("added Listener");
    }
}