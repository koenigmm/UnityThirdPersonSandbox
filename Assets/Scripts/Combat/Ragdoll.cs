using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    
    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>(true);
        _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (var collider in _colliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (var rigidbody in _rigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        characterController.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
