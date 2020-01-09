using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Manage the users stats (Currently only health)
/// </summary>
[ExecuteInEditMode]
public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float minHealth = 0;

    public float Health = 100f;
    
    /// <summary>
    /// Find a health display in the children and set their min and max health
    /// </summary>
    public void Start()
    {
        if (GetComponentInChildren<HealthDisplay>())
        {
            GetComponentInChildren<HealthDisplay>().minHealth = minHealth;
            GetComponentInChildren<HealthDisplay>().maxHealth = maxHealth;
        }        
    }

    /// <summary>
    ///Apply damage to the player
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        Health -= damage;
        if (GetComponentInChildren<HealthDisplay>())
        {
            GetComponentInChildren<HealthDisplay>().SetHealth(Health);
        }
    }

    /// <summary>
    /// Update the display and check if we're dead
    /// </summary>
    public void Update()
    {
        Health = Mathf.Clamp(Health, minHealth, maxHealth);
        if (GetComponentInChildren<HealthDisplay>())
        {
            GetComponentInChildren<HealthDisplay>().SetHealth(Health);
        }

        if (Health <= 0)
        {
            Die();
        }          
    }

    /// <summary>
    /// Actually kill player
    /// </summary>
    private void Die()
    {        
        GetComponentInChildren<DeathDisplay>(true).Die();
    }
}