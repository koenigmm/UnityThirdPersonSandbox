using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    public event Action OnDeadlyVelocity;
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] private float deadlyVelocityFactor = 300f;
    private float _verticalVelocity;
    private Vector3 _impact;
    private Vector3 _dampingVelocity;
    private bool _hasDeadlyVelocity;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

    private void Update()
    {
        if (controller.isGrounded && _hasDeadlyVelocity)
        {
            OnDeadlyVelocity?.Invoke();
            _hasDeadlyVelocity = false;
        }
        
        if (_verticalVelocity < 0f && controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        if (_verticalVelocity <= Physics.gravity.y * deadlyVelocityFactor * Time.deltaTime)
        {
            _hasDeadlyVelocity = true;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);

        if (_agent == null) return;
        if (_impact.sqrMagnitude < 0.2f * 0.2f)
        {
            _impact = Vector3.zero;
            _agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;

        if (_agent == null) return;

        if (_agent != null)
            _agent.enabled = false;
    }

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        _impact = Vector3.zero;
        _verticalVelocity = 0f;
    }
}