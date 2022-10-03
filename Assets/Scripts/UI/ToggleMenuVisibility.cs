using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenuVisibility : MonoBehaviour
{
    [SerializeField] private UIType userInterfaceType;
    [SerializeField] private bool canToggleOutsideZones;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas attributesUI;
    [SerializeField] private Button buttonToSelect;
    private PlayerStateMachine _playerStateMachine;
    private InteractionZoneHandler _interactionZoneHandler;
    private bool _showUI;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _interactionZoneHandler = player.GetComponent<InteractionZoneHandler>();
    }

    private void Start() => canvas.gameObject.SetActive(false);
    

    private void OnEnable()
    {
        switch (userInterfaceType)
        {
            case UIType.MainMenu:
                _playerStateMachine.InputReader.OnMainMenuToggle += ToggleUIVisibility;
                break;
            case UIType.LevelUp:
                _playerStateMachine.InputReader.InteractEvent += ToggleUIVisibility;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDisable()
    {
        switch (userInterfaceType)
        {
            case UIType.MainMenu:
                _playerStateMachine.InputReader.OnMainMenuToggle -= ToggleUIVisibility;
                break;
            case UIType.LevelUp:
                _playerStateMachine.InputReader.InteractEvent -= ToggleUIVisibility;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ToggleUIVisibility()
    {
        if (!_interactionZoneHandler.isInInteractionZone && !canToggleOutsideZones) return;

        _showUI = !_showUI;

        if (attributesUI != null) attributesUI.enabled = _showUI;
        canvas.gameObject.SetActive(_showUI);
        _interactionZoneHandler.HandleInterfaceChange();

        if (_showUI) buttonToSelect.Select();
        
    }

    [Serializable]
    private enum UIType
    {
        MainMenu,
        LevelUp
    }
}