using System;
using UnityEngine;
using TMPro;

public class LevelUpTokenDisplay : MonoBehaviour
{
    private LevelUpTokenInventory _levelUpTokenInventory;
    private TextMeshProUGUI _tokenCounterTextElement;

    private void Awake() => _levelUpTokenInventory = FindObjectOfType<LevelUpTokenInventory>();

    private void Start()
    {
        _tokenCounterTextElement = GetComponent<TextMeshProUGUI>();
        SetUIText();
    }
    
    private void OnEnable() => _levelUpTokenInventory.OnTokenValueChange += SetUIText;

    private void OnDisable() => _levelUpTokenInventory.OnTokenValueChange -= SetUIText;

    private void SetUIText() => _tokenCounterTextElement.text = _levelUpTokenInventory.AmountOfLevelUpTokens.ToString();
}
