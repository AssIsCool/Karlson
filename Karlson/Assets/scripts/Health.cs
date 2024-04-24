using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int curentHealth;
    public Healthbar healthBar;
    public static Health health;
    private void OnEnable()
    {
        health = this; 
    }
    private void OnDisable()
    {
        health = null;
    }

    private void Start()
    {
        curentHealth = maxHealth;
        healthBar.setMaxHealth(curentHealth);
        healthBar.setHealth(curentHealth);
    }
    private void Update()
    {
        if (curentHealth <= 0)
        {

        }
    }
    public void TakeDamage(int damage)
    {
        curentHealth -= damage;
        healthBar.setHealth(curentHealth);
    }
 
    private void BeingAttacked(int layer, GameObject go)
    {
        if (go.layer != layer)
            return;
        TakeDamage(20);
    }
    public void TakeLavaDamage(int damage)
    {
            // You can check if the entity type is a player before applying lava damage
            TakeDamage(damage);
            if (curentHealth <= 0)
                SceneManager.LoadScene("game over");
    }

}
