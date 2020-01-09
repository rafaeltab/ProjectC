using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the options menu
/// </summary>
[RequireComponent(typeof(Canvas))]
public class OptionsMenu : MonoBehaviour
{
    public List<IOptionsButton> OptionsPages { get; set; } = new List<IOptionsButton>();
    public List<SettingsPrefab> SettingPrefabs;
    
    [Space]
    public Rect displayField;
    public Button buttonTemplate;
    public Vector2 buttonLocations = new Vector2(0.125f, 0.5f);
    public float OptionsPadding = 10f;
    public GameObject eventSystem;

    /// <summary>
    /// Adds all the page buttons in the options menu
    /// </summary>
    public void Start()
    {
        OptionsPages.Add(new BackButton(buttonTemplate, GetComponent<Canvas>(),this,buttonLocations));
        int ind = 0;
        foreach(var settingsPage in SettingsManager.GetInstance().Settings)
        {
            OptionsPage op = new OptionsPage(buttonTemplate, settingsPage, GetComponent<Canvas>(), this, buttonLocations,ind);
            OptionsPages.Add(op);
            ind++;
        }
    }

    /// <summary>
    /// Opens a single options page
    /// </summary>
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

    /// <summary>
    /// Initiates rescaling of buttons when screen size changes
    /// </summary>
    public void OnRectTransformDimensionsChange()
    {
        Rescale();
    }

    /// <summary>
    /// Rescaling of the buttons
    /// </summary>
    public void Rescale()
    {
        int ind = -1;
        foreach (var optPage in OptionsPages)
        {
            optPage.Rescale(buttonTemplate,ind);
            ind++;
        }
    }
}

[System.Serializable]
public struct SettingsPrefab
{
    public string name;
    public GameObject prefab;
}