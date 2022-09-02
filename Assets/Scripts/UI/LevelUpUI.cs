using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private Canvas levelUpCanvas;
    [SerializeField] private Canvas attributesUI;
    private PlayerStateMachine _playerStateMachine;
    private bool _showUI = true;
    private InputReader _playerInputReader;
    private ForceReceiver _playerForceReceiver;
    private ThirdPersonCameraController _thirdPersonCameraController;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _playerInputReader = _playerStateMachine.InputReader;
        _playerForceReceiver = _playerStateMachine.ForceReceiver;
        _thirdPersonCameraController = _playerStateMachine.PlayerThirdPersonCameraController;
    }

    private void Start() => levelUpCanvas.enabled = false;

    private void OnEnable() => _playerStateMachine.InputReader.InteractEvent += ToggleUIVisibility;

    private void OnDisable() => _playerStateMachine.InputReader.InteractEvent -= ToggleUIVisibility;

    public void ToggleUIVisibility()
    {
        if (_showUI && _playerStateMachine.isInInteractionArea)
        {
            attributesUI.enabled = true;
            levelUpCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _playerStateMachine.enabled = false;
            _playerForceReceiver.enabled = false;
            _playerStateMachine.Animator.enabled = false;
            _thirdPersonCameraController.enabled = false;
        }
        else
        {
            levelUpCanvas.enabled = false;
            _playerForceReceiver.enabled = true;
            _playerStateMachine.enabled = true;
            _playerStateMachine.Animator.enabled = true;
            attributesUI.enabled = false;
            _thirdPersonCameraController.enabled = true;
        }

        _showUI = !_showUI;
    }
}