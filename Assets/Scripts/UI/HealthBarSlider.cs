using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (health != null) return;
        var player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDamage += UpdateHealthBar;
        health.OnHeal += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.OnDamage -= UpdateHealthBar;
        health.OnHeal -= UpdateHealthBar;
    }

    private void UpdateHealthBar() => slider.value = health.GetFraction();
}