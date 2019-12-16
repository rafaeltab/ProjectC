using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class SettingSerializer : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        Dictionary<string, object> allSettings = new Dictionary<string, object>();

        int index = 0;
        foreach (Settings s in SettingsManager.GetInstance().Settings)
        {
            foreach (Setting setting in s.SettingsList)
            {
                allSettings.Add($"{index} {setting.Id}", setting.Value);
            }
            index++;
        }

        string json = JsonConvert.SerializeObject(allSettings, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        });

        File.WriteAllText(@"SettingsTest.json", json);
        Debug.Log("Yeet");
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (File.Exists(@"SettingsTest.json"))
        {
            var settingsManager = SettingsManager.GetInstance();
            string json = File.ReadAllText(@"SettingsTest.json");
            var sm = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
            foreach (var item in sm)
            {
                var key = item.Key.Split(' ');
                int settings = Int32.Parse(key[0]);
                string setting = key[1];

                settingsManager.Settings[settings].SetSetting(setting, item.Value);
            }
            Debug.Log("Fetus Deletus");
        }
    }
}
