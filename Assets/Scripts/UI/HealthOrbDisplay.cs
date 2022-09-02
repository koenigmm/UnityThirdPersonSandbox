using UnityEngine;
using UnityEngine.UI;

public class HealthOrbDisplay : MonoBehaviour
{
    [SerializeField] private Image foregroundImage;
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        if (playerHealth != null) return;
        var player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        playerHealth.OnDamage += UpdateOrbImage;
        playerHealth.OnHeal += UpdateOrbImage;
    }

    private void OnDisable()
    {
        playerHealth.OnDamage -= UpdateOrbImage;
        playerHealth.OnHeal -= UpdateOrbImage;
    }
    
    private void UpdateOrbImage()
    {
        foregroundImage.fillAmount = playerHealth.GetFraction();
    }
}
