using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class OptionsMenu : MonoBehaviour
{
    public List<OptionsPage> OptionsPages { get; set; } = new List<OptionsPage>();
    public List<SettingsPrefab> SettingPrefabs;
    
    [Space]
    public Rect displayField;
    public Button buttonTemplate;
    public Vector2 buttonLocations = new Vector2(0.125f, 0.5f);

    public void Start()
    {
        int ind = 0;
        foreach(var settingsPage in SettingsManager.GetInstance().Settings)
        {
            OptionsPage op = new OptionsPage(buttonTemplate, settingsPage, GetComponent<Canvas>(), this, buttonLocations,ind);
            OptionsPages.Add(op);
            ind++;
        }
    }

    public void Update()
    {

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

    public void OnRectTransformDimensionsChange()
    {
        Rescale();
    }

    public void Rescale()
    {
        int ind = 0;
        foreach (var optPage in OptionsPages)
        {
<<<<<<< HEAD
            optPage.Rescale(buttonTemplate,ind);
            ind++;
=======
            pos.x = (1-sizePercentage.x)*Screen.width;
>>>>>>> 524d5e73905cf65e0a1234563ff087bf2ce1f4dd
        }
    }
}

[System.Serializable]
public struct SettingsPrefab
{
    public string name;
    public GameObject prefab;
}