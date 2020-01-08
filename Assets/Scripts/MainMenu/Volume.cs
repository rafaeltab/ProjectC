using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}