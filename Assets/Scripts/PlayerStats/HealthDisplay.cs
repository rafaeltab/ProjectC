using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The display for the health
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    public float maxHealth = 100;
    public float minHealth = 0;

    public Gradient barColor = new Gradient();

    public Image HealthBar;
    public TextMeshProUGUI textBar;

    /// <summary>
    /// Set the health we want to display
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        if (!(HealthBar == null || textBar == null))
        {
            var percentage = Mathf.InverseLerp(minHealth, maxHealth, health);
            HealthBar.color = barColor.Evaluate(percentage);
            HealthBar.transform.localScale = new Vector3(percentage, 1, 1);
            textBar.text = ((int)health).ToString();
        }        
    }
}
