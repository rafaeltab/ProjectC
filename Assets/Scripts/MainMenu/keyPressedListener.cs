using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Key listener
/// </summary>
public class keyPressedListener : MonoBehaviour
{
    private Setting setting;
    private GameObject eventSystem;

    /// <summary>
    /// Starts listening for input
    /// </summary>
    public void Run(Setting setting, GameObject eventSystem)
    {
        this.setting = setting;
        this.eventSystem = eventSystem;
    }

    /// <summary>
    /// Listen for input and destroys after input
    /// </summary>
    public void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            setting.Value = e.keyCode;
            eventSystem.SetActive(true);
            Destroy(this);
        }
    }
}