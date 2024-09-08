using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static event Action OnPlayerDeath;
    public float health, maxHealth;
    public HealthUI hpBar;
    

    void Start()
    {
        health = maxHealth;
        hpBar.SetHPMax(maxHealth);
    }

    public void TakeDmg(float damage){
        health -= damage;
        hpBar.SetHP(health);
        if(health <= 0){
            health = 0;
            OnPlayerDeath?.Invoke();
        }
    }

    public void HealDmg(float damage){
        health += damage;
        hpBar.SetHP(health);
        if(health > maxHealth){
            health = maxHealth;
        }
    }


}
