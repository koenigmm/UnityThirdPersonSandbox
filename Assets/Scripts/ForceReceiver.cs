using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float drag = 0.3f;
    private float _verticalVelocity;
    private Vector3 _impact;
    private Vector3 _dampingVelocity;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

    private void Update()
    {
        if (_verticalVelocity < 0f && controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
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
}