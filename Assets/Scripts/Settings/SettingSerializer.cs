using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

/// <summary>
/// Class used for putting all settings into a file and retrieving settigns from that file on load or quit
/// </summary>
public class SettingSerializer : MonoBehaviour
{
    /// <summary>
    /// Application quit so serialize everything and save
    /// </summary>
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

    /// <summary>
    /// Awake so we starting up which means we need to deserialize the file and put all the settigns in the right spot
    /// </summary>
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
