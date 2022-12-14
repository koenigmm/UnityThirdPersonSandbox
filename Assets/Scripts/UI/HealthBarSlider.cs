using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image fillImage;

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

    private void UpdateHealthBar(bool rangedDamage) => UpdateHealthBar();
    private void UpdateHealthBar() => fillImage.fillAmount = health.GetFraction();
}