using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Class that handles the volume slider
/// </summary>
public class Volume : MonoBehaviour
{
    private float master;
    public float Master
    {
        get
        {
            return master;
        }
    }

    /// <summary>
    /// checks the volume from Settings Manager and calls to change the volume
    /// </summary>
    private void Start()
    {
        //Volume
        Setting vol = SettingsManager.GetInstance().Settings[1].GetSetting("mainvol");
        vol.changeEvent += (sender, value) =>
        {
            master = (float)value;
            SetLevel(Master);
        };
        master = (float)vol.Value;
        SetLevel(Master);
    }
    public AudioMixer mixer;

    /// <summary>
    /// Changes the volume in the mixer
    /// </summary>
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}