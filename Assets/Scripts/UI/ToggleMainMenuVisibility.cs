using UnityEngine;

public class ToggleMainMenuVisibility : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;
    private bool _isActive;
    private InputReader _inputReader;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _inputReader = _playerStateMachine.GetComponent<InputReader>();
    }

    private void Start() => mainMenuCanvas.enabled = _isActive;

    private void OnEnable() => _inputReader.OnMainMenuToggle += ToggleVisibility;
    
    private void OnDisable() => _inputReader.OnMainMenuToggle -= ToggleVisibility;

    private void ToggleVisibility()
    {
        _isActive = !_isActive;
        mainMenuCanvas.enabled = _isActive;
        
        _playerStateMachine.SetPlayerControlsActive(!_isActive);
    }
}