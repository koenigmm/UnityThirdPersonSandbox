using UnityEngine;
using UnityEngine.UI;

public class ToggleMainMenuVisibility : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Button buttonToSelect;
    private bool _isActive;
    private InputReader _inputReader;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _inputReader = _playerStateMachine.GetComponent<InputReader>();
    }

    private void Start() => mainMenuCanvas.gameObject.SetActive(false);

    private void OnEnable() => _inputReader.OnMainMenuToggle += ToggleVisibility;
    
    private void OnDisable() => _inputReader.OnMainMenuToggle -= ToggleVisibility;

    private void ToggleVisibility()
    {
        _isActive = !_isActive;
        // mainMenuCanvas.enabled = _isActive;
        mainMenuCanvas.gameObject.SetActive(_isActive);
        
        _playerStateMachine.SetPlayerControlsActive(!_isActive);
        
        if (_isActive) buttonToSelect.Select();
    }
}