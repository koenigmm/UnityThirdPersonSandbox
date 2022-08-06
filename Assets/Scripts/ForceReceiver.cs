using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    private float _verticalVerlocity;
    
    public Vector3 Movement => Vector3.up * _verticalVerlocity;

    private void Update()
    {
        if (_verticalVerlocity < 0f && controller.isGrounded)
        {
            _verticalVerlocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVerlocity += Physics.gravity.y * Time.deltaTime;
        }
    }
    
}
