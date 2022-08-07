using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;
    private List<Collider> _alreadyCollidedWith = new List<Collider>();
    private float _damage;

    private void OnEnable()
    {
        _alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) return;
        if (_alreadyCollidedWith.Contains(other)) return;
        
        _alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out var health))
        {
            health.DealDamage(_damage);
        }
    }

    public void SetAttack(float damage)
    {
        _damage = damage;
    }
}
