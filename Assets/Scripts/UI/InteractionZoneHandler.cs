using System.Collections;
using UnityEngine;

public class InteractionZoneHandler : MonoBehaviour
{
    public bool isInInteractionZone;
    private PlayerStateMachine _playerStateMachine;
    private GameObject[] _interactableInterfaces;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _interactableInterfaces = GameObject.FindGameObjectsWithTag("CoreUI");
    }

    private void OnEnable() => StartCoroutine(SetupEventListener());

    private IEnumerator SetupEventListener()
    {
        yield return new WaitForEndOfFrame();
        _playerStateMachine.InputReader.OnMainMenuToggle += HandleInterfaceChange;
        _playerStateMachine.InputReader.InteractEvent += HandleInterfaceChange;
    }

    private void OnDisable()
    {
        _playerStateMachine.InputReader.OnMainMenuToggle -= HandleInterfaceChange;
        _playerStateMachine.InputReader.InteractEvent -= HandleInterfaceChange;
    }

    private void HandleInterfaceChange()
    {
        var activeElements = 0;
        foreach (var element in _interactableInterfaces)
        {
            if (element.activeSelf) activeElements++;
        }

        _playerStateMachine.SetPlayerControlsActive(activeElements == 0);
    }
}