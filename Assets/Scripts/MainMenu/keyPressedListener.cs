using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class keyPressedListener : MonoBehaviour
{
    private Setting setting;
    private GameObject eventSystem;
    public void Run(Setting setting, GameObject eventSystem)
    {
        this.setting = setting;
        this.eventSystem = eventSystem;
    }

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