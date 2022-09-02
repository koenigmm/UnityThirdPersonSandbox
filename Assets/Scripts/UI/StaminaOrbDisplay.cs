using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaOrbDisplay : MonoBehaviour
{
    [SerializeField] private Image foregroundImage;
    [SerializeField] private Stamina playerStamina;

    private void Awake()
    {
        if (playerStamina != null) return;
        var player = GameObject.FindWithTag("Player");
        playerStamina = player.GetComponent<Stamina>();
    }

    private void OnEnable() => playerStamina.OnStaminaChange += UpdateStaminaOrb;

    private void OnDisable() => playerStamina.OnStaminaChange -= UpdateStaminaOrb;

    private void UpdateStaminaOrb() => foregroundImage.fillAmount = playerStamina.GetStaminaFraction();
}
