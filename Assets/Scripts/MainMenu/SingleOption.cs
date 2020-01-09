using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for displaying and managing a single option
/// </summary>
public class SingleOption
{
    public GameObject settingObject;
    public Setting setting;
    public OptionsPage page;
    public int currentPage;
    public int settingInd;
    public Rect settingArea;
    public Canvas canvas;
    private GameObject prefab;
    private SettingsPrefab settingsPrefab;

    /// <summary>
    /// Constructor for a single option
    /// </summary>
    public SingleOption(Setting setting, OptionsPage page, List<SettingsPrefab> settingPrefabs, Transform parent, int pageInd, int settingInd, Rect settingArea,Canvas canvas)
    {
        this.canvas = canvas;
        this.setting = setting;
        this.page = page;
        this.currentPage = pageInd;
        this.settingInd = settingInd;
        this.settingArea = settingArea;

        settingsPrefab = settingPrefabs.Find((pre) =>
        {
            return pre.name.ToLower() == setting.Type.Name.ToLower();
        });

        GameObject prefab = settingsPrefab.prefab;
               
        this.prefab = prefab;
        if (prefab != null)
        {
            Resize(parent);
        }
        else
        {
            Debug.Log($"{setting.Type.Name.ToLower()}");
            prefab = settingPrefabs[0].prefab;
            this.prefab = prefab;
            Resize(parent);
        }
    }

    /// <summary>
    /// Sets the value of a setting and updates the text
    /// </summary>
    public void ValueChangedFloat(float val)
    {
        page.settings.SetSetting(setting.Id, val);
        settingObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{val}";
    }

    /// <summary>
    /// Listens for a change in the value of an option
    /// </summary>
    public void DoSingle()
    {
        settingObject.GetComponent<Slider>().onValueChanged.AddListener((val) => { ValueChangedFloat(val); });
    }

    /// <summary>
    /// Resizes the single option
    /// </summary>
    public void Resize(Transform parent)
    {
        bool wasNull = false;
        if (settingObject == null)
        {
            settingObject = Object.Instantiate(prefab,canvas.transform);
            wasNull = true;
        }

        settingObject.GetComponentInChildren<TextMeshProUGUI>().text = setting.Name;

        DoScale();
        DoPos();

        if (wasNull)
        {
            DoOptionTypes();
        }
    }

    /// <summary>
    /// Scales the single option
    /// </summary>
    public void DoScale()
    {
        Vector3 startScale = settingObject.transform.localScale;
        float widthPerc = (Screen.width * settingArea.width) / GetWidth(settingObject);
        startScale *= widthPerc;
        settingObject.transform.localScale = startScale;
    }

    /// <summary>
    /// Positions the single option
    /// </summary>
    public void DoPos()
    {
        Vector3 startPos = settingObject.transform.position;
        
        if (settingInd == 0)
        {
            startPos.y = Screen.height * (1 - settingArea.y);
        }
        else
        {
            SingleOption last = page.options[settingInd - 1];
            

            startPos.y = last.settingObject.transform.position.y - GetHeight(last.settingObject) - (Screen.height / (100 / page.optionsMenu.OptionsPadding));

        }
        startPos.x = Screen.width * settingArea.x;
        settingObject.transform.position = startPos;
    }

    /// <summary>
    /// Switch for the different types of options
    /// </summary>
    public void DoOptionTypes()
    {
        switch (settingsPrefab.name.ToLower())
        {
            case "single":
                DoSingleType();
                break;
            case "keycode":
                DoKeyCode();
                break;
            case "vector2":
                //vector2
                break;
            case "string":
                //string
                break;
            case "boolean":
                DoBoolType();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Listens for changes in the slider and the setting value
    /// </summary>
    public void DoSingleType()
    {
        settingObject.GetComponentInChildren<Slider>().onValueChanged.AddListener((value) => {
            SettingsManager.GetInstance().Settings[currentPage].SetSetting(setting.Id,value);
        });

        SettingsManager.GetInstance().Settings[currentPage].GetSetting(setting.Id).changeEvent += (sender, value) => {
            settingObject.GetComponentInChildren<Slider>().value = (float) value;
        };
    }

    /// <summary>
    /// Listens for changes in the toggle
    /// </summary>
    public void DoBoolType()
    {
        settingObject.GetComponentInChildren<Toggle>().isOn = (bool) setting.Value;
        settingObject.GetComponentInChildren<Toggle>().onValueChanged.AddListener((value)=> {
            setting.Value = value;
        });

        setting.changeEvent += (sender, value) => {
            settingObject.GetComponentInChildren<Toggle>().isOn = (bool)value;
        };
    }

    /// <summary>
    /// Starts a keycode listener
    /// </summary>
    public void DoKeyCode()
    {
        settingObject.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = setting.Value.ToString();
        settingObject.GetComponentInChildren<Button>().onClick.AddListener(()=> {
            settingObject.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "?";
            page.optionsMenu.eventSystem.SetActive(false);
            settingObject.AddComponent<keyPressedListener>().Run(setting,page.optionsMenu.eventSystem);
        });

        setting.changeEvent += (sender, value) =>
        {
            settingObject.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = ((KeyCode)value).ToString();
        };
    }

    /// <summary>
    /// Get height of given object
    /// </summary>
    public float GetHeight(GameObject obj)
    {
        return obj.GetComponent<RectTransform>().rect.height * obj.transform.localScale.y;
    }

    /// <summary>
    /// Get width of given object
    /// </summary>
    public float GetWidth(GameObject obj)
    {
        return obj.GetComponent<RectTransform>().rect.width * obj.transform.localScale.x;        
    }
}