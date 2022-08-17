using System;
using UnityEngine;
using TMPro;

public class LevelUpTokenDisplay : MonoBehaviour
{
    private LevelUpToken _levelUpToken;
    private TextMeshProUGUI _tokenCounterTextElement;

    private void Awake() => _levelUpToken = FindObjectOfType<LevelUpToken>();

    private void Start()
    {
        _tokenCounterTextElement = GetComponent<TextMeshProUGUI>();
        SetUIText();
    }
    
    private void OnEnable() => _levelUpToken.OnTokenValueChange += SetUIText;

    private void OnDisable() => _levelUpToken.OnTokenValueChange -= SetUIText;

    private void SetUIText() => _tokenCounterTextElement.text = _levelUpToken.AmountOfLevelUpTokens.ToString();
}
