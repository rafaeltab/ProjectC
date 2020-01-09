using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// SettingsManager (Singleton pattern used)
/// </summary>
public class SettingsManager
{
    public static SettingsManager _instance;
    private List<Settings> settings = new List<Settings>();

    /// <summary>
    /// List of settings pages, in order: Controls, Audio, Video
    /// </summary>
    public List<Settings> Settings
    {
        get { return settings; }
        private set { settings = value; }
    }

    /// <summary>
    /// Create an instance of the SettingsManager
    /// </summary>
    protected SettingsManager(string a)
    {
        settings.Add(new ControlsSettings());
        settings.Add(new AudioSettings());
        //settings.Add(new VideoSettings());
        settings.Add(new GeneralSettings());
    }

    /// <summary>
    /// Create an empty SettingsManager
    /// </summary>
    public SettingsManager(){}

    /// <summary>
    /// Get an instance of the SettingsManager
    /// </summary>
    public static SettingsManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SettingsManager("");
        }

        return _instance;
    }
}

/// <summary>
/// abstract class for a settings page
/// </summary>
public abstract class Settings
{
    private string className = "Default";
    public string ClassName
    {
        get;
        protected set;
    }

    private List<Setting> settings = new List<Setting>();
    /// <summary>
    /// the list of all setings. readonly unless inheriting
    /// </summary>
    public List<Setting> SettingsList
    {
        get { return settings; }
        protected set { settings = value; }
    }

    /// <summary>
    /// Set a settings value
    /// </summary>
    /// <param name="key">the settings key</param>
    /// <param name="value">the value to set the setting to</param>
    /// <exception cref="SettingNotFoundException">thrown when the settings key was not found</exception>
    /// <exception cref="SettingNotFoundException">thrown when the value supplied is not the correct type</exception>
    public void SetSetting(string key, object value)
    {

        Setting found = null;
        foreach (Setting s in settings)
        {
            if (s.Id == key)
            {
                found = s;
                break;
            }
        }

        if (found == null)
        {
            throw new SettingNotFoundException($"The setting {key} was not found");
        }

        if (found.Type.IsEnum)
        {
            found.Value = Enum.ToObject(found.Type, value);
        }
        else
        {
            found.Value = Convert.ChangeType(value, found.Type);
        }
    }

    /// <summary>
    /// Get a setting by setting key
    /// </summary>
    public Setting GetSetting(string key)
    {
        Setting setting = settings.Where(s => { return key == s.Id; }).ToArray()[0];

        return setting;
    }
}

/// <summary>
/// All control settings
/// </summary>
public class ControlsSettings : Settings {
    /// <summary>
    /// add the default settings
    /// </summary>
    public ControlsSettings()
    {
        ClassName = "Controls";
        SettingsList.Add(new Setting("Move Forward", "mforw", KeyCode.W, typeof(KeyCode)));
        SettingsList.Add(new Setting("Move Left", "mleft", KeyCode.A, typeof(KeyCode)));
        SettingsList.Add(new Setting("Move Backwards", "mback", KeyCode.S, typeof(KeyCode)));
        SettingsList.Add(new Setting("Move Right", "mright", KeyCode.D, typeof(KeyCode)));
        SettingsList.Add(new Setting("Jump", "jump", KeyCode.Space, typeof(KeyCode)));
        SettingsList.Add(new Setting("Crouch", "crouch", KeyCode.LeftControl, typeof(KeyCode)));
        SettingsList.Add(new Setting("Sprint", "sprint", KeyCode.LeftShift, typeof(KeyCode)));
        SettingsList.Add(new Setting("W-Dimension Up","wup", KeyCode.I,typeof(KeyCode)));
        SettingsList.Add(new Setting("W-Dimension Down", "wdown", KeyCode.J, typeof(KeyCode)));
        SettingsList.Add(new Setting("Open Inventory", "oinv", KeyCode.E, typeof(KeyCode)));
    }
} 

/// <summary>
/// All Audio settings 
/// </summary>
public class AudioSettings : Settings
{
    /// <summary>
    /// Add the main volume setting
    /// </summary>
    public AudioSettings()
    {
        ClassName = "Audio";
        SettingsList.Add(new Setting("Main Volume", "mainvol", 1f, typeof(float)));
        //SettingsList.Add(new Setting("Music Volume", "musicvol", "", typeof(float)));
        //SettingsList.Add(new Setting("Dialogue Volume", "dialvol", "", typeof(float)));
        //SettingsList.Add(new Setting("Effects Volume", "efxvol", "", typeof(float)));
    }
}

//public class VideoSettings : Settings
//{
//    public VideoSettings()
//    {
//        ClassName = "Video";
//        SettingsList.Add(new Setting("TestSetting", "test", "", typeof(string)));
//    }
//}


/// <summary>
/// all general settings
/// </summary>
public class GeneralSettings : Settings
{
    /// <summary>
    /// Add fps setting
    /// </summary>
    public GeneralSettings()
    {
        ClassName = "General";
        SettingsList.Add(new Setting("Show fps", "showfps", false, typeof(bool)));
    }
}

/// <summary>
/// Setting template for individual settings
/// </summary>
public class Setting
{
    /// <summary>
    /// Add a function with the right pattern to call it when value is changed
    /// </summary>
    [JsonIgnore]
    public EventHandler<object> changeEvent; 
    private object _value;

    /// <summary>
    /// The value of the setting
    /// </summary>
    public object Value
    {
        get { return _value; }
        set
        {
            _value = value;
            changeEvent?.Invoke(this, value);
        }
    }
    /// <summary>
    /// the settings name
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// The settigns type
    /// </summary>
    public Type Type { get; private set; }
    /// <summary>
    /// The settings id which is used to identify it
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Create a setting
    /// </summary>
    /// <param name="name">the name</param>
    /// <param name="id">the id used for referencing the setting</param>
    /// <param name="defaultValue">the default value of the setting</param>
    /// <param name="t">the type of the setting</param>
    public Setting(string name,string id, object defaultValue,Type t)
    {
        Type = t;
        Name = name;
        Id = id;
        Value = defaultValue;
    }
}