using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthValuesElement;
    private Stamina _stamina;
    private Slider _slider;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _stamina = player.GetComponent<Stamina>();

        _slider = GetComponent<Slider>();
    }

    private void OnEnable() => _stamina.OnStaminaChange += HandleStaminaChange;
    
    private void OnDisable() => _stamina.OnStaminaChange -= HandleStaminaChange;

    private void HandleStaminaChange()
    {
        _slider.value = _stamina.GetStaminaFraction();
        var stamina = MathF.Floor(_stamina.CurrentStamina);
        var staminaElementText = $"{stamina} / {_stamina.MaxStamina}";
        _healthValuesElement.text = staminaElementText;
    }
}