using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Enemy enemy;
    [Header("Health Color")]
    [SerializeField] private Color maxHealth;
    [SerializeField] private Color halfHealth;
    [SerializeField] private Color minHealth;

    private void Start()
    {
        enemy.OnHit += UpdateHealthBar;
        healthBar.color = maxHealth;
    }
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = enemy.Health / enemy.MaxHealth;
        if (healthBar.fillAmount > 0.7f)
        {
            healthBar.color = maxHealth;
        }
        else if (healthBar.fillAmount > 0.3f)
        {
            healthBar.color = halfHealth;
        }
        else
        {
            healthBar.color = minHealth;
        }
    }
}
