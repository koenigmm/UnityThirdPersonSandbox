using System;
using TMPro;
using UnityEngine;

public class AttributesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaGUIElement;
    [SerializeField] private TextMeshProUGUI healthGUIElement;
    private InputReader _inputReader;
    private Level _level;
    private Canvas _canvas;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _level = player.GetComponent<Level>();
        _inputReader = player.GetComponent<InputReader>();
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        HandleHealthLevelUpdate();
        HandleStaminaLevelUpdate();
        _canvas.enabled = false;
    }

    private void OnEnable()
    {
        _level.OnHealthLevelUp += HandleHealthLevelUpdate;
        _level.OnStaminaLevelUp += HandleStaminaLevelUpdate;
        _inputReader.ShowAttributesEvent += ToggleUIVisibility;
    }

    private void ToggleUIVisibility() => _canvas.enabled = !_canvas.enabled;

    private void OnDisable()
    {
        _level.OnHealthLevelUp -= HandleHealthLevelUpdate;
        _level.OnStaminaLevelUp -= HandleStaminaLevelUpdate;
    }

    private void HandleStaminaLevelUpdate() => staminaGUIElement.text = _level.GetUIStaminaLevel.ToString();

    private void HandleHealthLevelUpdate() => healthGUIElement.text = _level.GetUIHealthLevel.ToString();
}