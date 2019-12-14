using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public interface IOptionsButton
{
    bool IsBackButton();
    void Disable();
    void Rescale(Button pageButton, int ind, float oldWidth = 1920, float oldHeight = 1080);
    Button GetButton();
}