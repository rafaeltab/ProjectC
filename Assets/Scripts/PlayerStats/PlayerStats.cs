using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float minHealth = 0;

    public float Health = 100f;
    
    public void Start()
    {
        GetComponentInChildren<HealthDisplay>().minHealth = minHealth;
        GetComponentInChildren<HealthDisplay>().maxHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        GetComponentInChildren<HealthDisplay>().SetHealth(Health);
    }

    public void Update()
    {
        Health = Mathf.Clamp(Health, minHealth, maxHealth);
        GetComponentInChildren<HealthDisplay>().SetHealth(Health);
    }
}