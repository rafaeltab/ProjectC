using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public SingleOption(Setting setting, OptionsPage page, List<SettingsPrefab> settingPrefabs, Transform parent, int pageInd, int settingInd, Rect settingArea,Canvas canvas)
    {
        this.canvas = canvas;
        this.setting = setting;
        this.page = page;
        this.currentPage = pageInd;
        this.settingInd = settingInd;
        this.settingArea = settingArea;

        GameObject prefab = settingPrefabs.Find((pre) =>
        {
            return pre.name.ToLower() == setting.Type.Name.ToLower();
        }).prefab;
        this.prefab = prefab;
        if (prefab != null)
        {
            Resize(parent);
        }
        else
        {
            prefab = settingPrefabs[0].prefab;
            this.prefab = prefab;
            Resize(parent);
        }

       
    }

    public void ValueChangedFloat(float val)
    {
        page.settings.SetSetting(setting.Id, val);
        settingObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{val}";
    }

    public void DoSingle()
    {
        settingObject.GetComponent<Slider>().onValueChanged.AddListener((val) => { ValueChangedFloat(val); });
    }

    public void Resize(Transform parent)
    {
        if (settingObject == null)
        {
            settingObject = Object.Instantiate(prefab,canvas.transform);
        }
        else
        {
            settingObject.transform.position = prefab.transform.position;
            settingObject.transform.localScale = prefab.transform.localScale;
        }

        settingObject.GetComponentInChildren<TextMeshProUGUI>().text = setting.Name;

        DoScale();
        DoPos();
    }

    public void DoScale()
    {
        Vector3 startScale = settingObject.transform.localScale;
        float widthPerc = (Screen.width * settingArea.x) / GetWidth(settingObject);
        startScale /= widthPerc;
        settingObject.transform.localScale = startScale;
    }

    public void DoPos()
    {
        Vector3 startPos = settingObject.transform.position;

        if (settingInd == 0)
        {
            startPos.y = Screen.height * (1 - settingArea.y);
        }
        else
        {
            GameObject last = page.options[settingInd - 1].settingObject;
            startPos.y = last.transform.position.y - GetHeight(last);

        }
        startPos.x = Screen.width * settingArea.x;
        settingObject.transform.position = startPos;
    }

    public float GetHeight(GameObject obj)
    {
        return obj.GetComponent<RectTransform>().rect.height * obj.transform.transform.localScale.y;
    }

    public float GetWidth(GameObject obj)
    {
        return obj.GetComponent<RectTransform>().rect.width * obj.transform.transform.localScale.x;
    }
}