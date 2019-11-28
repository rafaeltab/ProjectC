using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public float maxHealth = 100;
    public float minHealth = 0;

    public Gradient barColor = new Gradient();

    public Image HealthBar;
    public TextMeshProUGUI textBar;

    public void SetHealth(float health)
    {
        var percentage = Mathf.InverseLerp(minHealth, maxHealth, health);
        HealthBar.color = barColor.Evaluate(percentage);
        HealthBar.transform.localScale = new Vector3(percentage,1,1);
        textBar.text = ((int)health).ToString();
    }
}
