using System;
using TMPro;
using UnityEngine;

public class HealthValueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthValueTextElement;
    [Tooltip("Default ist player health")]
    [SerializeField] private Health health;


    private void Awake()
    {
        if (health != null) return;
        var player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
    }
    
    private void OnEnable()
    {
        health.OnDamage += UpdateHealthValueDisplay;
        health.OnHeal += UpdateHealthValueDisplay;
    }

    private void OnDisable()
    {
        health.OnDamage -= UpdateHealthValueDisplay;
        health.OnHeal -= UpdateHealthValueDisplay;
    }
    
    private void UpdateHealthValueDisplay()
    {
        var currentHealth = MathF.Floor(health.CurrentHealth);
        var healthValue = $"{currentHealth} / {health.MaxHealth}";
        healthValueTextElement.text = healthValue;
    }

    private void UpdateHealthValueDisplay(bool rangedDamage) => UpdateHealthValueDisplay();
}