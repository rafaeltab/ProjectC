using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage
{
    public Button pageButton;
    public Settings settings;
    Canvas parentCanvas;
    OptionsMenu optionsMenu;
    Vector2 buttonLocations;
    public List<SingleOption> options = new List<SingleOption>();
    public int btnIndex;

    public OptionsPage(Button pageButton, Settings settings, Canvas parentCanvas, OptionsMenu optionsMenu, Vector2 buttonLocations, int btnIndex)
    {
        this.settings = settings;
        this.parentCanvas = parentCanvas;
        this.optionsMenu = optionsMenu;
        this.buttonLocations = buttonLocations;
        this.btnIndex = btnIndex;
        Rescale(pageButton, btnIndex);
    }

    public void Rescale(Button pageButton, int ind, float oldWidth = 1920, float oldHeight = 1080)
    {
        if (this.pageButton == null)
        {
            this.pageButton = Object.Instantiate(pageButton, parentCanvas.transform);
        }
        else
        {
            this.pageButton.transform.localScale = pageButton.transform.localScale;
        }

        float widthPerc = oldWidth / Screen.width;
        float heightPerc = oldHeight / Screen.height;

        Vector3 newScale = pageButton.transform.localScale;
        newScale.x /= widthPerc;
        newScale.y /= heightPerc;

        this.pageButton.transform.localScale = newScale;

        HandleButton(settings.ClassName, ind);

        if (options.Count == 0)
        {
            int inds = 0;
            foreach (var setting in this.settings.SettingsList)
            {
                //Instantiate correct object fort the type
                SingleOption option = new SingleOption(setting, this, optionsMenu.SettingPrefabs, this.pageButton.transform, btnIndex, inds, optionsMenu.displayField, parentCanvas);
                options.Add(option);
                inds++;
            }
        }
        else
        {
            foreach (var opt in options)
            {
                opt.Resize(parentCanvas.transform);
            }
        }
        
    }

    public void HandleClick()
    {
        optionsMenu.Enable(this);
        //display every setting in this.settings
    }

    public void Disable()
    {
        //remove the display of all the settings for this page
    }

    public void HandleButton(string name, int ind)
    {
        Vector3 pos = new Vector3(Screen.width * (buttonLocations.x), 0, 0);
        if (ind > 0)
        {
            OptionsPage last = optionsMenu.OptionsPages[ind - 1];
            pos.y = last.pageButton.transform.position.y + GetButtonHeight(last.pageButton);

            for (int i = 0; i < ind; i++)
            {
                Vector3 lastpos = optionsMenu.OptionsPages[i].pageButton.transform.position;
                lastpos.y -= GetButtonHeight(pageButton) / 2;
                optionsMenu.OptionsPages[i].pageButton.transform.position = lastpos;
            }

        }
        else
        {
            pos.y = (((1 - buttonLocations.y)) * Screen.height);
        }
        pageButton.transform.position = pos;

        pageButton.GetComponentInChildren<TextMeshProUGUI>().text = name;

        pageButton.onClick.AddListener(HandleClick);
    }

    public static float GetButtonHeight(Button btn)
    {
        return btn.GetComponent<RectTransform>().rect.height * btn.transform.localScale.y;
    }
}