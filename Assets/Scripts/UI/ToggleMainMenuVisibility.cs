using UnityEngine;

public class ToggleMainMenuVisibility : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;
    private bool _isActive;
    private InputReader _inputReader;

    private void Awake()
    {
        var playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _inputReader = playerStateMachine.GetComponent<InputReader>();
    }

    private void Start() => mainMenuCanvas.enabled = _isActive;

    private void OnEnable() => _inputReader.OnMainMenuToggle += ToggleVisibility;

    private void OnDisable() => _inputReader.OnMainMenuToggle += ToggleVisibility;

    private void ToggleVisibility()
    {
        _isActive = !_isActive;
        mainMenuCanvas.enabled = _isActive;
        Cursor.visible = _isActive;
        Cursor.lockState = _isActive ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
