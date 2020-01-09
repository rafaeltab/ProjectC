using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the options page
/// </summary>
public class OptionsPage : IOptionsButton
{
    public Button pageButton;
    public Settings settings;
    Canvas parentCanvas;
    public OptionsMenu optionsMenu;
    Vector2 buttonLocations;
    public List<SingleOption> options = new List<SingleOption>();
    public int btnIndex;

    public bool IsBackButton()
    {
        return false;
    }

    /// <summary>
    /// Constructor for option page
    /// </summary>
    public OptionsPage(Button pageButton, Settings settings, Canvas parentCanvas, OptionsMenu optionsMenu, Vector2 buttonLocations, int btnIndex)
    {
        this.settings = settings;
        this.parentCanvas = parentCanvas;
        this.optionsMenu = optionsMenu;
        this.buttonLocations = buttonLocations;
        this.btnIndex = btnIndex;
        Rescale(pageButton, btnIndex);
    }

    /// <summary>
    /// Rescales the elements in the options page
    /// </summary>
    /// <param name="pageButton"></param>
    /// <param name="ind"></param>
    /// <param name="oldWidth"></param>
    /// <param name="oldHeight"></param>
    public void Rescale(Button pageButton, int ind, float oldWidth = 1920, float oldHeight = 1080)
    {
        bool wasNull = false;
        if (this.pageButton == null)
        {
            wasNull = true;
            this.pageButton = Object.Instantiate(pageButton, parentCanvas.transform);
        }
        else
        {
            this.pageButton.transform.localScale = pageButton.transform.localScale;
            this.pageButton.transform.position = pageButton.transform.position;
        }

        float widthPerc = oldWidth / Screen.width;
        float heightPerc = oldHeight / Screen.height;

        Vector3 newScale = pageButton.transform.localScale;
        newScale.x /= widthPerc;
        newScale.y /= heightPerc;

        this.pageButton.transform.localScale = newScale;

        HandleButton(settings.ClassName, ind, !wasNull);

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

        if (ind == SettingsManager.GetInstance().Settings.Count - 1 && wasNull)
        {
            Enable();
        }
        else if (!wasNull)
        {

        }
        else
        {
            Disable();
        }
    }

    /// <summary>
    /// Handles clicking a button
    /// </summary>
    public void HandleClick()
    {
        optionsMenu.Enable(this);
        Enable();
    }

    /// <summary>
    /// Enables options to show in the options page
    /// </summary>
    public void Enable()
    {
        foreach (var option in options)
        {
            option.settingObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disables options to show in the options page
    /// </summary>
    public void Disable()
    {
        //remove the display of all the settings for this page
        foreach (var option in options)
        {
            option.settingObject.SetActive(false);
        }
    }

    /// <summary>
    /// Puts all the individual options in the options page
    /// </summary>
    public void HandleButton(string name, int ind, bool firstRun)
    {
        Vector3 pos = new Vector3(Screen.width * (buttonLocations.x), 0, 0);

        IOptionsButton last = optionsMenu.OptionsPages[ind];
        pos.y = last.GetButton().transform.position.y + GetButtonHeight(last.GetButton());

        for (int i = 0; i <= ind; i++)
        {
            Vector3 lastpos = optionsMenu.OptionsPages[i].GetButton().transform.position;
            lastpos.y -= GetButtonHeight(pageButton) / 2;
            optionsMenu.OptionsPages[i].GetButton().transform.position = lastpos;
        }

        pageButton.transform.position = pos;

        if (!firstRun)
        {
            pageButton.GetComponentInChildren<TextMeshProUGUI>().text = name;

            pageButton.onClick.AddListener(HandleClick);
        }
    }

    /// <summary>
    /// Grabs the height of a button
    /// </summary>
    public static float GetButtonHeight(Button btn)
    {
        return btn.GetComponent<RectTransform>().rect.height * btn.transform.localScale.y;
    }

    public Button GetButton()
    {
        return pageButton;
    }
}