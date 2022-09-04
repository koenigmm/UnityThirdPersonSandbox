using System;
using TMPro;
using UnityEngine;

public class StaminaValueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthValuesElement;
    private Stamina _stamina;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _stamina = player.GetComponent<Stamina>();
        
    }

    private void OnEnable() => _stamina.OnStaminaChange += HandleStaminaChange;
    
    private void OnDisable() => _stamina.OnStaminaChange -= HandleStaminaChange;

    private void HandleStaminaChange()
    {
        var stamina = MathF.Floor(_stamina.CurrentStamina);
        var staminaElementText = $"{stamina} / {_stamina.MaxStamina}";
        _healthValuesElement.text = staminaElementText;
    }
}