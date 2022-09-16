using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private Canvas levelUpCanvas;
    [SerializeField] private Canvas attributesUI;
    private PlayerStateMachine _playerStateMachine;
    private bool _showUI;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
    }

    private void Start() => levelUpCanvas.enabled = false;

    private void OnEnable() => _playerStateMachine.InputReader.InteractEvent += ToggleUIVisibility;

    private void OnDisable() => _playerStateMachine.InputReader.InteractEvent -= ToggleUIVisibility;

    public void ToggleUIVisibility()
    {
        if (!_playerStateMachine.isInInteractionArea) return;
        _showUI = !_showUI;
        attributesUI.enabled = _showUI;
        levelUpCanvas.enabled = _showUI;
        _playerStateMachine.SetPlayerControlsActive(!_showUI);

    }
}