using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    private Health _playerHealth;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerHealth = player.GetComponent<Health>();
        _slider = GetComponent<Slider>();
    }

    private void OnEnable() => _playerHealth.OnTakeDamage += UpdateHealthBar;

    private void OnDisable() => _playerHealth.OnTakeDamage -= UpdateHealthBar;

    private void UpdateHealthBar() => _slider.value = _playerHealth.GetFraction();
}
