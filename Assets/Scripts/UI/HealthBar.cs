using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthValueElement;
    private Slider _slider;
    private Health _playerHealth;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerHealth = player.GetComponent<Health>();
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDamage += UpdateHealthBar;
        _playerHealth.OnHeal += UpdateHealthBar;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamage -= UpdateHealthBar;
        _playerHealth.OnHeal -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        _slider.value = _playerHealth.GetFraction();
        var currentHealth = MathF.Floor(_playerHealth.CurrentHealth);
        var healthValue = $"{currentHealth} / {_playerHealth.MaxHealth}";
        _healthValueElement.text = healthValue;
    }
}
