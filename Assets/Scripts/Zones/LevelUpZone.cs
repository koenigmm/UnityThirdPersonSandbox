using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LevelUpZone : MonoBehaviour
{
    
    private PlayerStateMachine _stateMachine;
    private Canvas _hintCanvas;
    private bool _isTouchingPlayer;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        _hintCanvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        _hintCanvas.enabled = false;
    }

    private void Update()
    {
        if (!_isTouchingPlayer) return;
        var lookPos = _stateMachine.transform.position - _hintCanvas.transform.position;
        lookPos.y = 0f;
        var lookForwardRotation = Quaternion.LookRotation(lookPos);
        const float interpolationFactor = 2.5f;
        _hintCanvas.transform.rotation = Quaternion.Slerp(_hintCanvas.transform.rotation, lookForwardRotation, Time.deltaTime * interpolationFactor);
    }

    private void OnTriggerEnter(Collider other)
    {
        _isTouchingPlayer = other.TryGetComponent(out _stateMachine);
        if (!_isTouchingPlayer) return;
        HandleCampfireZone(other);
    }
    

    private void HandleCampfireZone(Component other)
    {
        if (_isTouchingPlayer)
        {
            _hintCanvas.enabled = true;
            _stateMachine.isInInteractionArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isTouchingPlayer)
        {
            _hintCanvas.enabled = false;
            _stateMachine.isInInteractionArea = false;
            _stateMachine = null;
        }

        _isTouchingPlayer = false;
    }
}