using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour
{
    private Stamina _stamina;
    private Slider _slider;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _stamina = player.GetComponent<Stamina>();

        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _stamina.OnStaminaChange += HandleStaminaChange;
    }


    private void OnDisable()
    {
        _stamina.OnStaminaChange -= HandleStaminaChange;
    }

    private void HandleStaminaChange()
    {
        _slider.value = _stamina.GetStaminaFraction();
    }
}