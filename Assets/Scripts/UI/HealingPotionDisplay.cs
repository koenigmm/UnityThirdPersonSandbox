using System;
using TMPro;
using UnityEngine;

public class HealingPotionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healingPotionGuiElement;
    private HealingPotionsInventory _healingPotionsInventory;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _healingPotionsInventory = player.GetComponent<HealingPotionsInventory>();
    }

    private void OnEnable() => _healingPotionsInventory.OnPotionUpdate += HandlePotionUpdate;

    private void OnDisable() => _healingPotionsInventory.OnPotionUpdate -= HandlePotionUpdate;

    private void Start() => HandlePotionUpdate();

    private void HandlePotionUpdate()
    {
        healingPotionGuiElement.text = _healingPotionsInventory.AmountOfPotions.ToString();
    }
    
}
